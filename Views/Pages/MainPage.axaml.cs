namespace OfficeTracker.Views.Pages;

public partial class MainPage : UserControl
{
    public MainPage()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    #region EVENTS

    private async void OnLoaded(object? sender, RoutedEventArgs e)
    {
	    if(DataContext is not MainPageViewModel mpv) return;

	    mpv.CurrentStatsChanged += CurrentStatsChanged;
	    await mpv.InitializeAsync();
    }

    private void CurrentStatsChanged(object? sender, EventArgs e)
	    => UpdateCurrentStats();

    #endregion

    #region CURRENT STATS

    private void UpdateCurrentStats()
    {
	    if (DataContext is not MainPageViewModel mpv) return;
	    if (mpv.CurrentStatsControl is null) return;

	    DynamicStatsContainer.Children.Clear();
	    DynamicStatsContainer.Children.Add(mpv.CurrentStatsControl);
    }

    #endregion
}
