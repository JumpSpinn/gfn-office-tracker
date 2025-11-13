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

	#region SELECTED TAB

	/// <summary>
	/// Handles the event triggered when the selected tab changes in the TabView. This method updates
	/// the selected tab index in the associated MainPageViewModel.
	/// </summary>
	private void OnTabSelectionChanged(object sender, SelectionChangedEventArgs args)
	{
		if(DataContext is not MainPageViewModel mpv) return;
		if (sender is not TabView tabView) return;
		mpv.UpdateSelectedTabIndex(tabView.SelectedIndex);
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
		if (sender is not PlannableDayListTemplate pdlc) return;
		mpv.ShowDeletePlannableDayDialogAsync(pdlc.SelectedPlannableDayId);
	}

	#endregion

	#region HOLIDAYS

	/// <summary>
	/// Handles the AddButtonClicked event from the HolidaysListTemplate control. This method invokes
	/// the ShowAddHolidayDialogAsync method in the associated MainPageViewModel to display a dialog
	/// for adding a new holiday.
	/// </summary>
	private void RequestShowAddHolidayDialog(object? sender, RoutedEventArgs e)
	{
		if(DataContext is not MainPageViewModel mpv) return;
		mpv.ShowAddHolidayDialogAsync();
	}

	/// <summary>
	/// Handles the DeleteButtonClicked event from the HolidaysListTemplate. This method invokes the
	/// ShowDeleteHolidayDialogAsync method in the associated MainPageViewModel to display a dialog
	/// for confirming and processing the deletion of a holiday.
	/// </summary>
	private void RequestShowDeleteHolidayDialog(object? sender, RoutedEventArgs e)
	{
		if(DataContext is not MainPageViewModel mpv) return;
		if (sender is not HolidaysListTemplate hlt) return;
		mpv.ShowDeleteHolidayDialogAsync(hlt.SelectedHolidayId);
	}

	#endregion


}

