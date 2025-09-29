namespace OfficeTracker.Views.Pages;

using Controls.Lists;

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

	    mpv.CurrentStatsChanged += CurrentStatsChanged;
	    await mpv.InitializeAsync();
    }

    /// <summary>
    /// Handles the CurrentStatsChanged event, which is triggered when there is a change in the current stats.
    /// This method updates the display of the current stats control on the MainPage.
    /// </summary>
    private void CurrentStatsChanged(object? sender, EventArgs e)
	    => UpdateCurrentStats();

    #endregion

    #region CURRENT STATS

    /// <summary>
    /// Updates the current stats display on the MainPage by clearing the existing contents
    /// of the dynamic stats container and adding the CurrentStatsControl from the ViewModel.
    /// Ensures the DataContext is of type MainPageViewModel and validates the presence of the CurrentStatsControl
    /// before performing the update.
    /// </summary>
    private void UpdateCurrentStats()
    {
	    if (DataContext is not MainPageViewModel mpv) return;
	    if (mpv.CurrentStatsControl is null) return;

	    DynamicStatsContainer.Children.Clear();
	    DynamicStatsContainer.Children.Add(mpv.CurrentStatsControl);
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
