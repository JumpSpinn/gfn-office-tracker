namespace OfficeTracker.Views.Pages;

/// <summary>
/// Represents the main page of the OfficeTracker application.
/// </summary>
public sealed partial class MainPage : UserControl
{
    public MainPage()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Invoked when the control is loaded and the associated visual tree is constructed.
    /// This method ensures that the MainPageViewModel asynchronous initialization is triggered.
    /// </summary>
    protected override void OnLoaded(RoutedEventArgs e)
    {
	    if(DataContext is MainPageViewModel mpv)
		    mpv.InitializeAsync();

	    base.OnLoaded(e);
    }
}
