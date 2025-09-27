namespace OfficeTracker.Views.Pages;

public partial class MainPage : UserControl
{
    public MainPage()
    {
        InitializeComponent();
        CalculateCurrentStats();
        DebugCalculatedWeeks();
    }

    #region CURRENT STATS

    private void CalculateCurrentStats()
    {
        var cs = new StatsControl()
        {
            HomeOfficeDays = 36,
            OfficeDays = 27
        };
        StatsGrid.Children.Add(cs);
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

    private void OpenModalToAddCurrentDay(object? sender, RoutedEventArgs e)
    {
        var dialog = new ContentDialog()
        {
            Title = "Heutigen Tag eintragen",
            Content = new CurrentDayForm(),
            PrimaryButtonText = "Eintragen",
            CloseButtonText = "Abbrechen",
            DefaultButton = ContentDialogButton.Primary
        };

        dialog.ShowAsyncCorrectly();
    }

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

        dialog.ShowAsyncCorrectly();
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

        dialog.ShowAsyncCorrectly();
    }


    #endregion

}
