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
        var stats = new StatsControl()
        {
            HomeOfficeDays = 10,
            OfficeDays = 20
        };

        var stats2 = new StatsControl()
        {
            HomeOfficeDays = 10,
            OfficeDays = 10
        };

        List.Items.Add(stats);
        List.Items.Add(stats2);
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
