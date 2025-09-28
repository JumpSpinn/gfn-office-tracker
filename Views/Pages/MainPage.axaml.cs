namespace OfficeTracker.Views.Pages;

public partial class MainPage : UserControl
{
	private readonly MainPageService _mainPageService;

    public MainPage()
    {
        InitializeComponent();

        _mainPageService = ((App)Application.Current!).Services?.GetRequiredService<MainPageService>()
	        ?? throw new InvalidOperationException("MainPageService not found");

        GetCurrentStatsAsync();
	    CalculateCurrentStats();
        DebugCalculatedWeeks();
    }

    #region CURRENT STATS

    private DbGeneral? _dbGeneralData;

    private async Task GetCurrentStatsAsync()
	    => _dbGeneralData = await _mainPageService.GetGeneralDataAsync();

    private async Task CalculateCurrentStats()
    {
	    DynamicStatsContainer.Children.Clear();

	    var cs = new StatsControl()
	    {
	        HomeOfficeDays = _dbGeneralData?.HomeOfficeDays ?? 0,
	        OfficeDays = _dbGeneralData?.OfficeDays ?? 0
	    };

	    DynamicStatsContainer.Children.Add(cs);
    }

    #endregion

    #region MODAL: ADD CURRENT DAY

    private void OpenModalToAddCurrentDay(object? sender, RoutedEventArgs e)
	    => ShowAddCurrentDayDialog();

    private async Task ShowAddCurrentDayDialog()
    {
	    if (_dbGeneralData is null) return;

	    var currentDayForm = new CurrentDayForm();
	    var dialog = new ContentDialog()
	    {
		    Title = "Heutigen Tag eintragen",
		    Content = currentDayForm,
		    PrimaryButtonText = "Eintragen",
		    CloseButtonText = "Abbrechen",
		    DefaultButton = ContentDialogButton.Primary
	    };

	    var dialogResult = await dialog.ShowAsyncCorrectly(MainPanel);
	    if (dialogResult == ContentDialogResult.Primary)
	    {
		    DayType? selectedType = currentDayForm.SelectedDayType;
		    switch (selectedType)
		    {
			    case DayType.HOME:
				    {
					    var dayResult = await _mainPageService.AddHomeOfficeDayAsync();
					    if (dayResult <= 0) return;

					    _dbGeneralData.HomeOfficeDays = dayResult;
					    break;
				    }
			    case DayType.OFFICE:
				    {
					    var dayResult = await _mainPageService.AddOfficeDayAsync();
					    if (dayResult <= 0) return;

					    _dbGeneralData.OfficeDays = dayResult;
					    break;
				    }
		    }

		    CalculateCurrentStats();
	    }
    }

    #endregion

    #region CALCULATED WEEKS

    private void DebugCalculatedWeeks()
    {
	    var week1 = new StatsControl(true)
	    {
		    OfficeDays = 31,
		    HomeOfficeDays = 37
	    };

	    var week2 = new StatsControl(true)
	    {
		    OfficeDays = 35,
		    HomeOfficeDays = 38
	    };

	    var week3 = new StatsControl(true)
	    {
		    OfficeDays = 39,
		    HomeOfficeDays = 39
	    };

	    var week4 = new StatsControl(true)
	    {
		    OfficeDays = 43,
		    HomeOfficeDays = 40
	    };

	    List.Items.Add(week1);
	    List.Items.Add(week2);
	    List.Items.Add(week3);
	    List.Items.Add(week4);
    }

    #endregion

    #region MODALS

    private void OpenModalToDeletePlannableDay(object? sender, RoutedEventArgs e)
    {
        var dialog = new ContentDialog()
        {
            Title = "Geplanten Tag löschen",
            Content = "Möchtest du diesen Eintrag wirklich löschen?",
            PrimaryButtonText = "Löschen",
            CloseButtonText = "Abbrechen",
            DefaultButton = ContentDialogButton.Primary
        };

        dialog.ShowAsyncCorrectly(MainPanel);
    }

    private void OpenModalToAddPlannableDay(object? sender, RoutedEventArgs e)
    {
        var dialog = new ContentDialog()
        {
            Title = "Eintrag hinzufügen",
            Content = new PlannableDayForm(),
            PrimaryButtonText = "Planen",
            CloseButtonText = "Abbrechen",
            DefaultButton = ContentDialogButton.Primary
        };

        dialog.ShowAsyncCorrectly(MainPanel);
    }


    #endregion

}
