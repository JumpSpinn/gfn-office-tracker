namespace OfficeTracker.Views.Pages;

/// <summary>
/// Represents the main page of the OfficeTracker application.
/// </summary>
public partial class MainPage : UserControl
{
    public MainPage()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Overrides the OnInitialized method to perform custom initialization for the MainPage.
    /// If the DataContext is of type MainPageViewModel, this method triggers the asynchronous
    /// initialization process of the ViewModel.
    /// </summary>
    protected override void OnInitialized()
    {
	    if(DataContext is MainPageViewModel mpv)
			mpv.InitializeAsync();

	    base.OnInitialized();
    }

    /// <summary>
    /// Handles the AddButtonClicked event from the StatisticControl. This method triggers the
    /// execution of an asynchronous operation to display a dialog for adding the current day's data.
    /// </summary>
    private void RequestShowAddCurrentDayDialog(object? sender, RoutedEventArgs e)
    {
	    if(DataContext is not MainPageViewModel mpv) return;
	    mpv.ShowAddCurrentDayDialogAsync();
    }

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
}
