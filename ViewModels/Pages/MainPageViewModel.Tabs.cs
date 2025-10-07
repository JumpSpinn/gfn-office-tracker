namespace OfficeTracker.ViewModels.Pages;

/// <summary>
/// Represents the view model for the main page, managing application logic and interactions
/// for the main page view components.
/// </summary>
public sealed partial class MainPageViewModel
{
	#region PLANNABLE DAYS

    [ObservableProperty]
    private ObservableCollection<DbPlannableDay> _plannableDays = [];

    /// <summary>
    /// Removes a plannable day from the collection based on the specified identifier.
    /// </summary>
    private void RemovePlannableDayFromCollection(uint id)
    {
	    var pd = PlannableDays.FirstOrDefault(x => x.Id == id);
	    if (pd is null) return;

	    var currentCollection = PlannableDays;
	    currentCollection.Remove(pd);
	    PlannableDays = new ObservableCollection<DbPlannableDay>(currentCollection);
    }

    /// <summary>
    /// Adds a plannable day to the collection if it does not already exist.
    /// </summary>
    private void AddPlannableDayToCollection(DbPlannableDay pd)
    {
	    var exist = PlannableDays.FirstOrDefault(x => x.Id == pd.Id);
	    if (exist is not null) return;

	    var currentCollection = PlannableDays;
	    currentCollection.Add(pd);
	    PlannableDays = new ObservableCollection<DbPlannableDay>(currentCollection);
    }

    /// <summary>
    /// Asynchronously loads the plannable days data by retrieving it from the MainPageService
    /// and updates the ViewModel's collection of plannable days.
    /// </summary>
    private async Task LoadPlannableDaysAsync()
    {
	    var plannableDays = await _databaseService.GetAllPlannableDaysAsync();
	    PlannableDays = new ObservableCollection<DbPlannableDay>(plannableDays ?? []);
    }

    /// <summary>
    /// Asynchronously shows a confirmation dialog to delete a plannable day and processes the deletion if confirmed.
    /// </summary>
    /// <param name="id">The unique identifier of the plannable day to be deleted.</param>
    public async Task ShowDeletePlannableDayDialogAsync(uint id)
    {
	    var dialog = new ContentDialog()
	    {
		    Title = "Geplanten Tag löschen",
		    Content = "Möchtest du diesen Eintrag wirklich löschen?",
		    PrimaryButtonText = "Löschen",
		    CloseButtonText = "Abbrechen",
		    DefaultButton = ContentDialogButton.Close
	    };

	    var dialogResult = await dialog.ShowAsyncCorrectly();
	    if(dialogResult == ContentDialogResult.Primary)
	    {
		    var deleted = await _databaseService.DeletePlannableDayAsync(id);
		    if(!deleted)
			    await DialogHelper.ShowDialogAsync("Eintrag löschen", "Eintrag konnte nicht gelöscht werden.", DialogType.ERROR);
		    else
		    {
			    RemovePlannableDayFromCollection(id);
			    await ReCalculateWeeksAsync();
		    }
	    }
    }

    /// <summary>
    /// Asynchronously displays a dialog for adding a new plannable day, validates the user input,
    /// and updates the list of plannable days if a valid new entry is created.
    /// </summary>
    public async Task ShowAddPlannableDayDialogAsync()
    {
	    var dayForm = new PlannableDayForm();
	    var dialog = new ContentDialog()
	    {
		    Title = "Eintrag hinzufügen",
		    Content = dayForm,
		    PrimaryButtonText = "Planen",
		    CloseButtonText = "Abbrechen",
		    DefaultButton = ContentDialogButton.Close
	    };

	    var success = false;
	    var result = await dialog.ShowAsyncCorrectly();
	    if (result == ContentDialogResult.Primary)
	    {
		    var dateValidation = IsSelectedDateValid(dayForm.SelectedDate);
		    if(!dateValidation.Result)
			    await DialogHelper.ShowDialogAsync(dateValidation.Title, dateValidation.Message, DialogType.WARNING);
		    else if(await _databaseService.GetSinglePlannableDayAsync((DateTime)dayForm.SelectedDate!) is not null)
			    await DialogHelper.ShowDialogAsync("Duplikat", "Diesen Tag hast du bereits geplant!", DialogType.WARNING);
		    else if (dayForm.SelectedDayType == DayType.NONE)
			    await DialogHelper.ShowDialogAsync("Höö?", "Du hast was anderes ausgewählt als HomeOffice oder Standort?!", DialogType.ERROR);
		    else if(dayForm.SelectedDayType == DayType.HOME && DateTimeHelper.IsDateInDayArray((DateTime)dayForm.SelectedDate!, _mainWindowService.RuntimeData.HomeOfficeDays))
			    await DialogHelper.ShowDialogAsync("Achtung", "Du planst einen HomeOffice Tag an einem regulären HomeOffice Tag.", DialogType.QUESTION);
		    else if(dayForm.SelectedDayType == DayType.OFFICE && DateTimeHelper.IsDateInDayArray((DateTime)dayForm.SelectedDate!, _mainWindowService.RuntimeData.OfficeDays))
			    await DialogHelper.ShowDialogAsync("Achtung", "Du planst einen Standort Tag an einem regulären Standort Tag.", DialogType.QUESTION);
		    else
		    {
			    var entry = await _databaseService.CreatePlannableDayAsync(dayForm.SelectedDayType, (DateTime)dayForm.SelectedDate!);
			    if (entry is not null)
			    {
				    success = true;
				    AddPlannableDayToCollection(entry);
				    await ReCalculateWeeksAsync();
				    await DialogHelper.ShowDialogAsync("Eintrag hinzugefügt", "Eintrag wurde erfolgreich gespeichert.", DialogType.SUCCESS);
			    }
			    else
				    await DialogHelper.ShowDialogAsync("Fehler", "Eintrag konnte nicht gespeichert werden.", DialogType.ERROR);
		    }
	    }
	    else
			success = true;

	    if (!success)
		    await ShowAddPlannableDayDialogAsync();
    }

    #endregion

    #region CALCULATED WEEKS

    [ObservableProperty]
    private ObservableCollection<CalculatedWeekModel> _calculatedWeekModels = new();

    [ObservableProperty]
    private decimal? _calculateWeeksCount = 1;

    /// <summary>
    /// Asynchronously recalculates the weeks by invoking the calculation service and updates
    /// the collection of calculated week models in the ViewModel.
    /// </summary>
    private async Task ReCalculateWeeksAsync()
    {
	    var cws = await _calculateWeekService.CalculateWeeksAsync();
	    if (cws is null) return;
	    CalculatedWeekModels = new ObservableCollection<CalculatedWeekModel>(cws);
    }

    /// <summary>
    /// Asynchronously calculates new week data based on user-defined input or generates the next week if
    /// no input is provided, and updates the collection of calculated weeks in the ViewModel.
    /// </summary>
    [RelayCommand]
    private async Task AddNewCalculatedWeekAsync()
    {
	    List<CalculatedWeekModel> cwsTotal = [];

	    if (CalculateWeeksCount is not null)
	    {
		    var cws = await _calculateWeekService.CalculateWeeksCountAsync((decimal)CalculateWeeksCount);
		    if(cws is not null)
			    cwsTotal.AddRange(cws);
	    }
	    else
	    {
		    var single = await _calculateWeekService.CalculateNextWeekAsync();
		    if(single is not null)
			    cwsTotal.Add(single);
	    }

	    CalculatedWeekModels = new ObservableCollection<CalculatedWeekModel>(CalculatedWeekModels.Concat(cwsTotal));
    }

    #endregion
}
