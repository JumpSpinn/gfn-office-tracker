namespace OfficeTracker.Views.Pages;

public partial class MainPage : UserControl
{
    public MainPage()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private void OnLoaded(object? sender, RoutedEventArgs e)
    {
	    CalculateCurrentStats();
	    DebugCalculatedWeeks();
    }

    #region CURRENT STATS

    private async Task CalculateCurrentStats()
    {
	    if (DataContext is not MainPageViewModel mpv) return;

	    var newControl = await mpv.CreateNewStatsControl();
	    if(newControl is null) return;

	    DynamicStatsContainer.Children.Clear();
	    DynamicStatsContainer.Children.Add(newControl);
    }

    #endregion

    #region MODAL: ADD CURRENT DAY

    private void OpenModalToAddCurrentDay(object? sender, RoutedEventArgs e)
	    => ShowAddCurrentDayDialogAsync();

    private async Task ShowAddCurrentDayDialogAsync()
    {
	    if (DataContext is not MainPageViewModel mpv) return;

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
					    await mpv.AddHomeOfficeDayAsync();
					    break;
				    }
			    case DayType.OFFICE:
				    {
					    await mpv.AddOfficeDayAsync();
					    break;
				    }
		    }

		    CalculateCurrentStats();
	    }
    }

    #endregion

    #region MODAL: ADD PLANNABLE DAY

    private void OpenModalToAddPlannableDay(object? sender, RoutedEventArgs e)
	    => ShowAddPlannableDayDialogAsync();

    private async Task ShowAddPlannableDayDialogAsync()
    {
	    if (DataContext is not MainPageViewModel mpv) return;

	    var dayForm = new PlannableDayForm();
	    var dialog = new ContentDialog()
	    {
		    Title = "Eintrag hinzufügen",
		    Content = dayForm,
		    PrimaryButtonText = "Planen",
		    CloseButtonText = "Abbrechen",
		    DefaultButton = ContentDialogButton.Primary
	    };

	    var dialogResult = await dialog.ShowAsyncCorrectly(MainPanel);
	    if (dialogResult == ContentDialogResult.Primary)
	    {
		    var selectedType = dayForm.SelectedDayType;
		    var selectedDate = dayForm.GetSelectedDate;

		    if (selectedType == DayType.NONE) return;

		    var plannableDay = await mpv.CreatePlannableDayAsync(selectedType, selectedDate);
		    // TODO: add handling alla irgendwo liste und so

		    Console.WriteLine($"Ausgewählter Tag: {selectedType}");
		    Console.WriteLine($"Ausgewähltes Datum: {selectedDate}");
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

    #endregion

}
