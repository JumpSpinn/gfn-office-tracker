namespace OfficeTracker.ViewModels.Pages;

[RegisterSingleton]
public sealed partial class MainPageViewModel : ViewModelBase
{
	private readonly MainPageService _mainPageService;

    public MainPageViewModel(MainPageService mps)
    {
	    _mainPageService = mps;
    }

    public async Task InitializeAsync()
    {
	    await CreateNewStatsControlAsync();
	    await LoadPlannableDaysAsync();
    }

    #region CURRENT STATS

    [ObservableProperty]
    private StatsControl? _currentStatsControl;

    public event EventHandler? CurrentStatsChanged;

    private async Task CreateNewStatsControlAsync()
    {
	    var data = await _mainPageService.GetGeneralDataAsync();
	    if (data is null) return;

	    CurrentStatsControl = new StatsControl()
	    {
		    HomeOfficeDays = data.HomeOfficeDays,
		    OfficeDays = data.OfficeDays
	    };

	    CurrentStatsChanged?.Invoke(this, EventArgs.Empty);;
    }

    #endregion

    #region BLUR EFFECT

    [ObservableProperty]
    private Effect? _blurEffect;

    private void EnableBlurEffect()
	    => BlurEffect = new BlurEffect() { Radius = Options.MODAL_BLUR_RADIUS };

    private void DisableBlurEffect()
		=> BlurEffect = null;

    #endregion

    #region CURRENT DAY

    [RelayCommand]
    private async Task ShowAddCurrentDayDialogAsync()
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

	    var dialogResult = await dialog.ShowAsyncCorrectly();
	    if (dialogResult == ContentDialogResult.Primary)
	    {
		    if (dayForm.SelectedDayType == DayType.NONE)
			    await DialogHelper.ShowDialog("Höö?", "Du hast was anderes ausgewählt als HomeOffice oder Standort?!");
		    else if(DateTimeHelper.IsInWeekend(DateTime.Today))
			    await DialogHelper.ShowDialog("Wochenende", "Du hast Wochenende, genieß' es.");
		    else if(dayForm.SelectedDayType == DayType.HOME)
			    await _mainPageService.AddHomeOfficeDayAsync();
		    else
			    await _mainPageService.AddOfficeDayAsync();
		    await CreateNewStatsControlAsync();
	    }
	    DisableBlurEffect();
    }

    #endregion

    #region PLANNABLE DAYS

    [ObservableProperty]
    private ObservableCollection<DbPlannableDay> _plannableDays = [];

    private async Task LoadPlannableDaysAsync()
    {
	    var plannableDays = await _mainPageService.GetPlannableDaysAsync();
	    PlannableDays = new ObservableCollection<DbPlannableDay>(plannableDays ?? []);
    }

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

	    var result = await dialog.ShowAsyncCorrectly();
	    if (result == ContentDialogResult.Primary)
	    {
		    var dateValidation = IsSelectedDateValid(dayForm.SelectedDate);
		    if(!dateValidation.Result)
			    await DialogHelper.ShowDialog(dateValidation.Title, dateValidation.Message);
		    else if (dayForm.SelectedDayType == DayType.NONE)
			    await DialogHelper.ShowDialog("Höö?", "Du hast was anderes ausgewählt als HomeOffice oder Standort?!");
		    else if(dayForm.SelectedDayType == DayType.HOME && DateTimeHelper.IsHomeOfficeDay((DateTime)dayForm.SelectedDate!))
			    await DialogHelper.ShowDialog("Achtung", "Du planst einen HomeOffice Tag an einem regulären HomeOffice Tag.");
		    else if(dayForm.SelectedDayType == DayType.OFFICE && DateTimeHelper.IsOfficeDay((DateTime)dayForm.SelectedDate!))
			    await DialogHelper.ShowDialog("Achtung", "Du planst einen Standort Tag an einem regulären Standort Tag.");
		    else
		    {
			    await _mainPageService.CreatePlannableDayAsync(dayForm.SelectedDayType, (DateTime)dayForm.SelectedDate!);
			    await LoadPlannableDaysAsync();
		    }
	    }
	    DisableBlurEffect();
    }

    #endregion

    #region VALIDATION

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
