namespace OfficeTracker.Views.Pages;

public partial class MainPage : UserControl
{
    public MainPage()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    #region LOADED

    private void OnLoaded(object? sender, RoutedEventArgs e)
    {
	    CalculateCurrentStats();
    }

    #endregion

    #region CURRENT STATS

    private async Task CalculateCurrentStats()
    {
	    if (DataContext is not MainPageViewModel mpv) return;

	    var newControl = await mpv.CreateNewStatsControlAsync();
	    if(newControl is null) return;

	    DynamicStatsContainer.Children.Clear();
	    DynamicStatsContainer.Children.Add(newControl);
    }

    #endregion
}
