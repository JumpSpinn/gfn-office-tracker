namespace OfficeTracker.Features.Pages.Main.Views;

/// <summary>
/// Represents the tabs component on the main page of the application.
/// </summary>
public partial class MainPageTabs : UserControl
{
	public MainPageTabs()
	{
		InitializeComponent();
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
		if (sender is not PlannableDayListTemplate pdlc) return;
		mpv.ShowDeletePlannableDayDialogAsync(pdlc.SelectedPlannableDayId);
	}
}

