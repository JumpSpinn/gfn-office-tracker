namespace OfficeTracker.Views.Pages;

/// <summary>
/// Represents the main page of the OfficeTracker application.
/// </summary>
public partial class MainPage : UserControl
{
    public MainPage()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    #region EVENTS

    /// <summary>
    /// Handles the Loaded event for the MainPage. This method sets up event subscriptions
    /// and initializes the associated ViewModel asynchronously.
    /// </summary>
    private async void OnLoaded(object? sender, RoutedEventArgs e)
    {
	    if(DataContext is not MainPageViewModel mpv) return;
	    mpv.InitializeAsync();
    }

    #endregion

    #region CURRENT DAY

    /// <summary>
    /// Handles the AddButtonClicked event from the StatisticControl. This method triggers the
    /// execution of an asynchronous operation to display a dialog for adding the current day's data.
    /// </summary>
    private void RequestShowAddCurrentDayDialog(object? sender, RoutedEventArgs e)
    {
	    if(DataContext is not MainPageViewModel mpv) return;
	    mpv.ShowAddCurrentDayDialogAsync();
    }

    #endregion

    #region PLANNABLE DAYS

    /// <summary>
    /// Handles the AddButtonClicked event from the PlannableDayListControl. This method invokes the
    /// ShowAddPlannableDayDialogAsync method in the associated MainPageViewModel to display a dialog
    /// for adding a new plannable day.
    /// </summary>
    private void RequestShowAddPlannableDayDialog(object? sender, RoutedEventArgs e)
    {
	    if(DataContext is not MainPageViewModel mpv) return;
	    mpv.ShowAddPlannableDayDialogAsync();
    }

    /// <summary>
    /// Handles the DeleteButtonClicked event for the PlannableDayListControl. This method invokes
    /// the ViewModel to display a dialog for confirming the deletion of a selected plannable day.
    /// </summary>
    private void RequestShowDeletePlannableDayDialog(object? sender, RoutedEventArgs e)
    {
	    if(DataContext is not MainPageViewModel mpv) return;
	    if (sender is not PlannableDayListControl pdlc) return;
	    mpv.ShowDeletePlannableDayDialogAsync(pdlc.SelectedPlannableDayId);
    }

    #endregion
}
