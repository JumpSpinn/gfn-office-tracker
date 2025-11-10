namespace OfficeTracker.Features.Pages.Main.ViewModels;

/// <summary>
/// Represents the view model for the main page, managing application logic and interactions
/// for the main page view components.
/// </summary>
public sealed partial class MainPageViewModel
{
	#region PLANNABLE DAYS

    [ObservableProperty]
    private ObservableCollection<PlannableDayModel> _plannableDays = [];

    /// <summary>
    /// Removes a plannable day from the collection based on the specified identifier.
    /// </summary>
    private void RemovePlannableDayFromCollection(uint id)
    {
	    var pd = PlannableDays.FirstOrDefault(x => x.Id == id);
	    if (pd is null) return;

	    var currentCollection = PlannableDays;
	    currentCollection.Remove(pd);
	    PlannableDays = new ObservableCollection<PlannableDayModel>(currentCollection);
    }

    /// <summary>
    /// Adds a plannable day to the collection if it does not already exist.
    /// </summary>
    private void AddPlannableDayToCollection(PlannableDayModel pd)
    {
	    var exist = PlannableDays.FirstOrDefault(x => x.Id == pd.Id);
	    if (exist is not null) return;

	    var currentCollection = PlannableDays;
	    currentCollection.Add(pd);
	    PlannableDays = new ObservableCollection<PlannableDayModel>(currentCollection);
    }

    /// <summary>
    /// Asynchronously loads the plannable days data by retrieving it from the MainPageService
    /// and updates the ViewModel's collection of plannable days.
    /// </summary>
    private async Task LoadPlannableDaysAsync()
    {
	    var plannableDays = await _databaseService.GetAllPlannableDaysAsync();
	    PlannableDays = new ObservableCollection<PlannableDayModel>(plannableDays ?? []);
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
		    var dateValidation = IsSelectedPlannableDateValid(dayForm.SelectedDate);
		    if(!dateValidation.Result)
			    await DialogHelper.ShowDialogAsync(dateValidation.Title, dateValidation.Message, DialogType.WARNING);
		    else if(await _databaseService.GetSinglePlannableDayByDateAsync((DateTime)dayForm.SelectedDate!) is not null)
			    await DialogHelper.ShowDialogAsync("Duplikat", "Diesen Tag hast du bereits geplant!", DialogType.WARNING);
		    else if (dayForm.SelectedDayType == DayType.NONE)
			    await DialogHelper.ShowDialogAsync("Höö?", "Du hast was anderes ausgewählt als HomeOffice oder Standort?!", DialogType.ERROR);
		    else if(dayForm.SelectedDayType == DayType.HOME && DateTimeHelper.IsDateInDayArray((DateTime)dayForm.SelectedDate!, _mainWindowController.RuntimeDataEntity.HomeOfficeDays))
			    await DialogHelper.ShowDialogAsync("Achtung", "Du planst einen HomeOffice Tag an einem regulären HomeOffice Tag.", DialogType.QUESTION);
		    else if(dayForm.SelectedDayType == DayType.OFFICE && DateTimeHelper.IsDateInDayArray((DateTime)dayForm.SelectedDate!, _mainWindowController.RuntimeDataEntity.OfficeDays))
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

    #region HOLIDAYS

    /// <summary>
    /// Represents a collection of holidays used within the main page view model.
    /// This collection contains instances of <see cref="HolidayModel"/>, which define
    /// individual holiday details such as name, start date, and end date. It is utilized
    /// to manage and display holiday information in the application's main page.
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<HolidayModel> _holidays = [];

    /// <summary>
    /// Removes a holiday from the collection based on the specified identifier.
    /// </summary>
    private void RemoveHolidayFromCollection(uint id)
    {
	    var pd = Holidays.FirstOrDefault(x => x.Id == id);
	    if (pd is null) return;

	    var currentCollection = Holidays;
	    currentCollection.Remove(pd);
	    Holidays = new ObservableCollection<HolidayModel>(currentCollection);
    }

    /// <summary>
    /// Adds a holiday to the collection if it is not already present.
    /// </summary>
    private void AddHolidayToCollection(HolidayModel pd)
    {
	    var exist = Holidays.FirstOrDefault(x => x.Id == pd.Id);
	    if (exist is not null) return;

	    var currentCollection = Holidays;
	    currentCollection.Add(pd);
	    Holidays = new ObservableCollection<HolidayModel>(currentCollection);
    }

    /// <summary>
    /// Loads the list of holidays from the database and updates the Holidays collection.
    /// </summary>
    private async Task LoadHolidaysAsync()
    {
	    var holidays = await _databaseService.GetAllHolidaysAsync();
	    Holidays = new ObservableCollection<HolidayModel>(holidays ?? []);
    }

    /// <summary>
    /// Displays a dialog for adding a new holiday, allowing the user to specify the holiday details such as name, start date, and end date.
    /// </summary>
    public async Task ShowAddHolidayDialogAsync()
    {
	    var holidayForm = new HolidayForm();
	    var dialog = new ContentDialog()
	    {
		    Title = "Neuen Urlaub eintragen",
		    Content = holidayForm,
		    PrimaryButtonText = "Eintragen",
		    CloseButtonText = "Abbrechen",
		    DefaultButton = ContentDialogButton.Close
	    };

	    var success = false;
	    var result = await dialog.ShowAsyncCorrectly();
	    if (result == ContentDialogResult.Primary)
	    {
		    var name = holidayForm.HolidayName.Text;
		    var startDate = holidayForm.SelectedStartDate;
		    var endDate = holidayForm.SelectedEndDate;

		    if(name is null || name.Trim().Length <= 0)
			    await DialogHelper.ShowDialogAsync("Urlaubsname", "Bitte gib einen Urlaubsname ein.", DialogType.ERROR);
		    else if(startDate is null)
			    await DialogHelper.ShowDialogAsync("Startdatum", "Bitte gib ein Startdatum ein.", DialogType.ERROR);
		    else if(endDate is null)
			    await DialogHelper.ShowDialogAsync("Enddatum", "Bitte gib ein Enddatum ein.", DialogType.ERROR);
		    else if(DateTimeHelper.IsInPast((DateTime)startDate) && DateTimeHelper.IsInPast((DateTime)endDate))
			    await DialogHelper.ShowDialogAsync("Urlaubszeitraum", "Der Urlaubszeitraum muss in der Zukunft liegen.", DialogType.ERROR);
		    else
		    {
			    var entry = await _databaseService.CreateHolidayAsync(name, (DateTime)startDate, (DateTime)endDate);
			    if (entry is not null)
			    {
				    success = true;
				    AddHolidayToCollection(entry);
				    await ReCalculateWeeksAsync();
				    await DialogHelper.ShowDialogAsync("Urlaub eingetragen", "Urlaub wurde erfolgreich gespeichert.", DialogType.SUCCESS);
			    }
			    else
				    await DialogHelper.ShowDialogAsync("Fehler", "Urlaub konnte nicht gespeichert werden.", DialogType.ERROR);
		    }
	    }
	    else
		    success = true;

	    if(!success)
		    await ShowAddHolidayDialogAsync();
    }

    /// <summary>
    /// Displays a dialog allowing the user to confirm the deletion of a holiday.
    /// If confirmed, the holiday is removed from the database and the collection is updated.
    /// </summary>
    public async Task ShowDeleteHolidayDialogAsync(uint id)
    {
	    var dialog = new ContentDialog()
	    {
		    Title = "Urlaub löschen",
		    Content = "Möchtest du diesen Eintrag wirklich löschen?",
		    PrimaryButtonText = "Löschen",
		    CloseButtonText = "Abbrechen",
		    DefaultButton = ContentDialogButton.Close
	    };

	    var dialogResult = await dialog.ShowAsyncCorrectly();
	    if(dialogResult == ContentDialogResult.Primary)
	    {
		    var deleted = await _databaseService.DeleteHolidayAsync(id);
		    if(!deleted)
			    await DialogHelper.ShowDialogAsync("Eintrag löschen", "Eintrag konnte nicht gelöscht werden.", DialogType.ERROR);
		    else
		    {
			    RemoveHolidayFromCollection(id);
			    await ReCalculateWeeksAsync();
		    }
	    }
    }

    #endregion

    #region CALCULATED WEEKS

    [ObservableProperty]
    private ObservableCollection<CalculatedWeekEntity> _calculatedWeekModels = new();

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
	    CalculatedWeekModels = new ObservableCollection<CalculatedWeekEntity>(cws);
    }

    /// <summary>
    /// Asynchronously calculates new week data based on user-defined input or generates the next week if
    /// no input is provided, and updates the collection of calculated weeks in the ViewModel.
    /// </summary>
    [RelayCommand]
    private async Task AddNewCalculatedWeekAsync()
    {
	    List<CalculatedWeekEntity> cwsTotal = [];

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

	    CalculatedWeekModels = new ObservableCollection<CalculatedWeekEntity>(CalculatedWeekModels.Concat(cwsTotal));
    }

    #endregion
}
