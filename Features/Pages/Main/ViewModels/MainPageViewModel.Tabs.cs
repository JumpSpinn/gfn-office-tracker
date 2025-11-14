namespace OfficeTracker.Features.Pages.Main.ViewModels;

/// <summary>
/// Represents the view model for the main page, managing application logic and interactions
/// for the main page view components.
/// </summary>
public sealed partial class MainPageViewModel
{
	#region SELECTED TAB

	[ObservableProperty]
	private int _selectedTabIndex;

	/// <summary>
	/// Updates the selected tab index and synchronizes it with the application configuration.
	/// </summary>
	public async Task UpdateSelectedTabIndex(int index)
	{
		SelectedTabIndex = index;
		await LoadTabDataAsync((TabType)index);

		if (_configController.ConfigEntity.SelectedTab == index) return;
		if (!_configController.ConfigEntity.RememberSelectedTabIndex) return;

		_configController.ConfigEntity.SelectedTab = SelectedTabIndex;
		_configController.SaveConfigToFile();
	}

	#endregion

	#region PLANNABLE DAYS

	[ObservableProperty]
	private ObservableCollection<PlannableDayModel> _plannableDays = [];

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
		    if (!deleted)
			    await _messageBoxController.ShowErrorAsync("Eintrag konnte nicht gelöscht werden.");
		    else
		    {
			    RemovePlannableDayFromCollection(id);
			    await LoadTabDataAsync(TabType.CALCULATED_WEEKS);
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
	    var selectedDate = dayForm.SelectedDate;

	    if (result == ContentDialogResult.Primary)
	    {
		    if(selectedDate is null)
			    await _messageBoxController.ShowWarningAsync("Ungültiges Datum. Du hast das Datum vergessen.");
		    else
		    {
			    var dateValidation = IsSelectedPlannableDateValid((DateTime)selectedDate);
			    if(!dateValidation.Result)
				    await _messageBoxController.ShowWarningAsync($"{dateValidation.Title}\n{dateValidation.Message}");
			    else if(await _databaseService.GetSinglePlannableDayByDateAsync((DateTime)selectedDate) is not null)
				    await _messageBoxController.ShowWarningAsync("Diesen Tag hast du bereits geplant!");
			    else if (dayForm.SelectedDayType == DayType.NONE)
				    await _messageBoxController.ShowWarningAsync("Du hast was anderes ausgewählt als HomeOffice oder Standort?!");
			    else if(dayForm.SelectedDayType == DayType.HOME && DateTimeHelper.IsDateInDayArray((DateTime)selectedDate, _mainWindowController.RuntimeDataEntity.HomeOfficeDays))
				    await _messageBoxController.ShowWarningAsync("Du planst einen HomeOffice Tag an einem regulären HomeOffice Tag.");
			    else if(dayForm.SelectedDayType == DayType.OFFICE && DateTimeHelper.IsDateInDayArray((DateTime)selectedDate, _mainWindowController.RuntimeDataEntity.OfficeDays))
				    await _messageBoxController.ShowWarningAsync("Du planst einen Standort Tag an einem regulären Standort Tag.");
			    else
			    {
				    var entry = await _databaseService.CreatePlannableDayAsync(dayForm.SelectedDayType, (DateTime)selectedDate);
				    if (entry is not null)
				    {
					    success = true;
					    AddPlannableDayToCollection(entry);
					    await LoadTabDataAsync(TabType.CALCULATED_WEEKS);
					    await _messageBoxController.ShowSuccessAsync("Eintrag wurde erfolgreich gespeichert.");
				    }
				    else
					    await _messageBoxController.ShowErrorAsync("Eintrag konnte nicht gespeichert werden.");
			    }
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
    /// Loads the list of holidays from the database and updates the Holidays collection.
    /// </summary>
    private async Task LoadHolidaysAsync()
    {
	    var holidays = await _databaseService.GetAllHolidaysAsync();
	    Holidays = new ObservableCollection<HolidayModel>(holidays ?? []);
    }

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
			    await _messageBoxController.ShowWarningAsync("Bitte gib einen Urlaubsname ein.");
		    else if(startDate is null)
			    await _messageBoxController.ShowWarningAsync("Bitte gib ein Startdatum ein.");
		    else if(endDate is null)
			    await _messageBoxController.ShowWarningAsync("Bitte gib ein Enddatum ein.");
		    else if(DateTimeHelper.IsInPast((DateTime)startDate) && DateTimeHelper.IsInPast((DateTime)endDate))
			    await _messageBoxController.ShowWarningAsync("Der Urlaubszeitraum muss in der Zukunft liegen.");
		    else if(await _databaseService.GetSingleHolidayByStartEndDateAsync((DateTime)startDate, (DateTime)endDate) is not null)
			    await _messageBoxController.ShowWarningAsync("Urlaubszeitraum wurde bereits eingetragen.");
		    else
		    {
			    var entry = await _databaseService.CreateHolidayAsync(name, (DateTime)startDate, (DateTime)endDate);
			    if (entry is not null)
			    {
				    success = true;
				    AddHolidayToCollection(entry);
				    await LoadTabDataAsync(TabType.CALCULATED_WEEKS);
				    await _messageBoxController.ShowSuccessAsync("Urlaub wurde erfolgreich gespeichert.");
			    }
			    else
				    await _messageBoxController.ShowErrorAsync("Urlaub konnte nicht gespeichert werden.");
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
			    await _messageBoxController.ShowErrorAsync("Eintrag konnte nicht gelöscht werden.");
		    else
		    {
			    RemoveHolidayFromCollection(id);
			    await LoadTabDataAsync(TabType.CALCULATED_WEEKS);
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
