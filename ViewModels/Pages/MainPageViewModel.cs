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

	    var currentDayForm = new CurrentDayForm();
	    var dialog = new ContentDialog()
	    {
		    Title = "Heutigen Tag eintragen",
		    Content = currentDayForm,
		    PrimaryButtonText = "Eintragen",
		    CloseButtonText = "Abbrechen",
		    DefaultButton = ContentDialogButton.Primary
	    };

	    var dialogResult = await dialog.ShowAsyncCorrectly();
	    if (dialogResult == ContentDialogResult.Primary)
	    {
		    DayType? selectedType = currentDayForm.SelectedDayType;
		    switch (selectedType)
		    {
			    case DayType.HOME:
				    {
					    await _mainPageService.AddHomeOfficeDayAsync();
					    break;
				    }
			    case DayType.OFFICE:
				    {
					    await _mainPageService.AddOfficeDayAsync();
					    break;
				    }
		    }
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

	    var dialogResult = await dialog.ShowAsyncCorrectly();
	    if (dialogResult == ContentDialogResult.Primary)
	    {
		    var selectedType = dayForm.SelectedDayType;
		    var selectedDate = dayForm.GetSelectedDate;

		    if (selectedType == DayType.NONE) return;

		    var plannableDayEntry = await _mainPageService.CreatePlannableDayAsync(selectedType, selectedDate);
		    if (plannableDayEntry is null) return;

		    await LoadPlannableDaysAsync();
	    }
	    DisableBlurEffect();
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
}
