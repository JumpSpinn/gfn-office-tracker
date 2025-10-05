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
    }

    #region CALCULATED WEEKS

    [ObservableProperty] private ObservableCollection<CalculatedWeekModel> _calculatedWeekModels =
	    new ObservableCollection<CalculatedWeekModel>();

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

    private bool _notifyToAddCurrentDay;

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
	    _notifyToAddCurrentDay = CanAddCurrentDay;

#if DEBUG
	    CanAddCurrentDay = true;
#else
	    CanAddCurrentDay = !DateTimeHelper.IsToday(data.Value.lastUpdate);
#endif

	    if (_notifyToAddCurrentDay)
		    DialogHelper.ShowDialogAsync($"Willkommen zurück, {_mainWindowService.RuntimeData.UserName}", "Vergiss nicht den heutigen Tag einzutragen!", DialogType.INFO);
    }

    #endregion

    #region CURRENT DAY

    /// <summary>
    /// Asynchronously displays a dialog to log the current day as either a home office day or office day,
    /// handles the selection, and updates the application's state accordingly.
    /// </summary>
    public async Task ShowAddCurrentDayDialogAsync()
    {
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
    }

    #endregion

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
		    DefaultButton = ContentDialogButton.Primary
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
