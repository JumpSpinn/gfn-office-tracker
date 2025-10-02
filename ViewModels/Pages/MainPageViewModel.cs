namespace OfficeTracker.ViewModels.Pages;

/// <summary>
/// Represents the ViewModel for the main page, providing functionality to manage and interact with
/// the main page's data and behavior in an MVVM architecture.
/// </summary>
[RegisterSingleton]
public sealed partial class MainPageViewModel : ViewModelBase
{
	private readonly MainPageService _mainPageService;
	private readonly LogService _logService;

    public MainPageViewModel(MainPageService mps, LogService lc)
    {
	    _mainPageService = mps;
	    _logService = lc;
    }

    /// <summary>
    /// Asynchronously initializes the state of the MainPageViewModel by creating a new stats control
    /// and loading plannable days data.
    /// </summary>
    public async Task InitializeAsync()
    {
	    await SetStatisticsAsync();
	    await LoadPlannableDaysAsync();
	    _logService.Debug($"Initialized with HomeOffice Days: {HomeOfficeDays}, Office Days: {OfficeDays}, Plannable Days Count: {PlannableDays.Count}, CanAddCurrentDay: {CanAddCurrentDay}");
    }

    #region CURRENT STATS

    [ObservableProperty]
    private uint _homeOfficeDays;

    [ObservableProperty]
    private uint _officeDays;

    [ObservableProperty]
    private bool _canAddCurrentDay;

    /// <summary>
    /// Asynchronously creates and initializes a new stats control using the general data
    /// retrieved from the MainPageService, updating the current stats control and related states.
    /// Triggers the CurrentStatsChanged event upon successful update.
    /// </summary>
    private async Task SetStatisticsAsync()
    {
	    var data = await _mainPageService.GetGeneralDataAsync();
	    if (data is null) return;

	    HomeOfficeDays = data.HomeOfficeDays;
	    OfficeDays = data.OfficeDays;

		#if DEBUG
	    CanAddCurrentDay = true;
		#else
		CanAddCurrentDay = !DateTimeHelper.IsToday(data.LastUpdate);
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
		    else if(dayForm.SelectedDayType == DayType.HOME)
			    entryResult = await _mainPageService.AddHomeOfficeDayAsync();
		    else
			    entryResult = await _mainPageService.AddOfficeDayAsync();
		    if (entryResult > 0)
		    {
			    await SetStatisticsAsync();
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
	    var plannableDays = await _mainPageService.GetPlannableDaysAsync();
	    PlannableDays = new ObservableCollection<DbPlannableDay>(plannableDays ?? []);
    }

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
		    await _mainPageService.DeletePlannableDayAsync(id);
		    await LoadPlannableDaysAsync();
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
		    else if(await _mainPageService.ExistPlannableDayDateAsync((DateTime)dayForm.SelectedDate!))
			    await DialogHelper.ShowDialogAsync("Duplikat", "Diesen Tag hast du bereits geplant!", DialogType.WARNING);
		    else if (dayForm.SelectedDayType == DayType.NONE)
			    await DialogHelper.ShowDialogAsync("Höö?", "Du hast was anderes ausgewählt als HomeOffice oder Standort?!", DialogType.ERROR);
		    else if(dayForm.SelectedDayType == DayType.HOME && DateTimeHelper.IsHomeOfficeDay((DateTime)dayForm.SelectedDate!))
			    await DialogHelper.ShowDialogAsync("Achtung", "Du planst einen HomeOffice Tag an einem regulären HomeOffice Tag.", DialogType.QUESTION);
		    else if(dayForm.SelectedDayType == DayType.OFFICE && DateTimeHelper.IsOfficeDay((DateTime)dayForm.SelectedDate!))
			    await DialogHelper.ShowDialogAsync("Achtung", "Du planst einen Standort Tag an einem regulären Standort Tag.", DialogType.QUESTION);
		    else
		    {
			    var entry = await _mainPageService.CreatePlannableDayAsync(dayForm.SelectedDayType, (DateTime)dayForm.SelectedDate!);
			    if (entry is not null)
			    {
				    success = true;
				    await LoadPlannableDaysAsync();
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
