namespace OfficeTracker.ViewModels.Pages;

/// <summary>
/// Represents the ViewModel for the main page, providing functionality to manage and interact with
/// the main page's data and behavior in an MVVM architecture.
/// </summary>
[RegisterSingleton]
public sealed partial class MainPageViewModel : ViewModelBase
{
	private readonly LogService _logService;
	private readonly DatabaseService _databaseService;
	private readonly MainWindowService _mainWindowService;
	private readonly CalculateWeekService _calculateWeekService;

    public MainPageViewModel(DatabaseService ds, LogService lc, MainWindowService mws, CalculateWeekService cws)
    {
	    _logService = lc;
	    _databaseService = ds;
	    _mainWindowService = mws;
	    _calculateWeekService = cws;
    }

    /// <summary>
    /// Asynchronously initializes the state of the MainPageViewModel by creating a new stats control
    /// and loading plannable days data.
    /// </summary>
    public async Task InitializeAsync()
    {
	    await ReCalculateWeeksAsync();
	    await RefreshStatisticsAsync();
	    await LoadPlannableDaysAsync();
	    _logService.Debug($"Initialized with HomeOffice Days: {HomeOfficeDays}, Office Days: {OfficeDays}, Plannable Days Count: {PlannableDays.Count}, CanAddCurrentDay: {CanAddCurrentDay}");
    }

    #region CALCULATED WEEKS

    [ObservableProperty]
    private ObservableCollection<CalculatedWeekModel> _calculatedWeekModels;

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
    /// Asynchronously adds a new calculated week using the CalculateWeekService and updates the collection
    /// of CalculatedWeekModels if the calculation produces a valid result.
    /// </summary>
    [RelayCommand]
    private async Task AddNewCalculatedWeekAsync()
    {
	    var cw = await _calculateWeekService.CalculateNextWeekAsync();
	    if (cw is null) return;
	    CalculatedWeekModels = new ObservableCollection<CalculatedWeekModel>(CalculatedWeekModels.Append(cw));
    }

    #endregion

    #region QUOTE GETTER

    public uint HomeOfficeQuote
	    => _mainWindowService.RuntimeData.HomeOfficeTargetQuoted;

    public uint OfficeQuote
	    => _mainWindowService.RuntimeData.OfficeTargetQuoted;

    #endregion

    #region CURRENT STATS

    [ObservableProperty]
    private uint _homeOfficeDays;

    [ObservableProperty]
    private uint _officeDays;

    [ObservableProperty]
    private bool _canAddCurrentDay;

    /// <summary>
    /// Asynchronously refreshes statistical data by retrieving and updating the counts for
    /// home office days, office days, and determining whether the current day can be added
    /// based on the last recorded update.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation of refreshing statistics.
    /// </returns>
    private async Task RefreshStatisticsAsync()
    {
	    var data = await _databaseService.GetDayCountsFromUserSettingsAsync();
	    if (data is null)
	    {
		    _logService.Error("Day counts could not be retrieved.");
		    return;
	    }

	    HomeOfficeDays = data.Value.homeOfficeCount;
	    OfficeDays = data.Value.officeCount;
	    CanAddCurrentDay = !DateTimeHelper.IsToday(data.Value.lastUpdate);

		#if DEBUG
	    CanAddCurrentDay = true;
		#endif
    }

    #endregion

    #region CURRENT DAY

    /// <summary>
    /// Asynchronously displays a dialog to log the current day as either a home office day or office day,
    /// handles the selection, and updates the application's state accordingly.
    /// </summary>
    public async Task ShowAddCurrentDayDialogAsync()
    {
	    EnableBlurEffect();

	    var dayForm = new CurrentDayForm();
	    var dialog = new ContentDialog()
	    {
		    Title = "Heutigen Tag eintragen",
		    Content = dayForm,
		    PrimaryButtonText = "Eintragen",
		    CloseButtonText = "Abbrechen",
		    DefaultButton = ContentDialogButton.Primary
	    };

	    uint entryResult = 0;
	    var dialogResult = await dialog.ShowAsyncCorrectly();
	    if (dialogResult == ContentDialogResult.Primary)
	    {
		    if (dayForm.SelectedDayType == DayType.NONE)
			    await DialogHelper.ShowDialogAsync("Höö?", "Du hast was anderes ausgewählt als HomeOffice oder Standort?!", DialogType.ERROR);
		    else if(DateTimeHelper.IsInWeekend(DateTime.Today))
			    await DialogHelper.ShowDialogAsync("Wochenende", "Du hast Wochenende, genieß' es.", DialogType.QUESTION);
		    else if (dayForm.SelectedDayType == DayType.HOME)
			    entryResult = await _databaseService.IncreaseHomeOfficeCountAsync();
		    else
			    entryResult = await _databaseService.IncreaseOfficeCountAsync();
		    if (entryResult > 0)
		    {
			    await RefreshStatisticsAsync();
			    await ReCalculateWeeksAsync();
			    await DialogHelper.ShowDialogAsync("Eingetragen", "Dein heutiger Tag wurde aufgenommen. Alle Statistiken wurden aktualisiert!", DialogType.SUCCESS);
		    }
		    else
			    await DialogHelper.ShowDialogAsync("Fehler", "Eintrag konnte nicht gespeichert werden.", DialogType.ERROR);
	    }
	    DisableBlurEffect();
    }

    #endregion

    #region PLANNABLE DAYS

    [ObservableProperty]
    private ObservableCollection<DbPlannableDay> _plannableDays = [];

    /// <summary>
    /// Asynchronously loads the plannable days data by retrieving it from the MainPageService
    /// and updates the ViewModel's collection of plannable days.
    /// </summary>
    private async Task LoadPlannableDaysAsync()
    {
	    var plannableDays = await _databaseService.GetAllPlannableDaysAsync();
	    PlannableDays = new ObservableCollection<DbPlannableDay>(plannableDays ?? []);
    }

    #endregion

    #region DELETE PLANNABLE DAY

    /// <summary>
    /// Asynchronously shows a confirmation dialog to delete a plannable day and processes the deletion if confirmed.
    /// </summary>
    /// <param name="id">The unique identifier of the plannable day to be deleted.</param>
    public async Task ShowDeletePlannableDayDialogAsync(uint id)
    {
	    EnableBlurEffect();

	    var dialog = new ContentDialog()
	    {
		    Title = "Geplanten Tag löschen",
		    Content = "Möchtest du diesen Eintrag wirklich löschen?",
		    PrimaryButtonText = "Löschen",
		    CloseButtonText = "Abbrechen",
		    DefaultButton = ContentDialogButton.Primary
	    };

	    var dialogResult = await dialog.ShowAsyncCorrectly();
	    if(dialogResult == ContentDialogResult.Primary)
	    {
		    await _databaseService.DeletePlannableDayAsync(id); // TODO: remove entry from list instead of load the hole fucking list again
		    await LoadPlannableDaysAsync();
		    await ReCalculateWeeksAsync();
	    }

	    DisableBlurEffect();
    }

    #endregion

    #region ADD PLANNABLE DAY

    /// <summary>
    /// Asynchronously displays a dialog for adding a new plannable day, validates the user input,
    /// and updates the list of plannable days if a valid new entry is created.
    /// </summary>
    public async Task ShowAddPlannableDayDialogAsync()
    {
	    EnableBlurEffect();

	    var dayForm = new PlannableDayForm();
	    var dialog = new ContentDialog()
	    {
		    Title = "Eintrag hinzufügen",
		    Content = dayForm,
		    PrimaryButtonText = "Planen",
		    CloseButtonText = "Abbrechen",
		    DefaultButton = ContentDialogButton.Primary
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
				    await LoadPlannableDaysAsync(); // TODO: add entry to list instead of load the hole list again
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
	    {
		    await ShowAddPlannableDayDialogAsync();
		    return;
	    }

	    DisableBlurEffect();
    }

    #endregion

    #region VALIDATION

    /// <summary>
    /// Validates whether a selected date is valid based on specific conditions, such as being non-null,
    /// not in the past, not a weekend, and not the current date.
    /// </summary>
    /// <param name="dt">The date to validate, or null if no date is provided.</param>
    /// <returns>
    /// A tuple containing a boolean indicating validation success, a title string for error messages,
    /// and a descriptive message. If the validation succeeds, the title and message will be empty.
    /// </returns>
    private (bool Result, string Title, string Message) IsSelectedDateValid(DateTime? dt)
    {
	    if(dt is null)
		    return (false, "Ungültiges Datum", "Du hast das Datum vergessen.");
	    else if(DateTimeHelper.IsToday((DateTime)dt))
		    return (false, "Ungültiges Datum", "Den heutigen Tag kannst du nicht mehr planen.");
	    else if(DateTimeHelper.IsInPast((DateTime)dt))
		    return (false, "Ungültiges Datum", "Der Tag liegt in der Vergangenheit.");
	    else if(DateTimeHelper.IsInWeekend((DateTime)dt))
		    return (false, "Ungültiges Datum", "Du arbeitst am Wochenende?");
	    return (true, "", "");
    }

    #endregion
}
