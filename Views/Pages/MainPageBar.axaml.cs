namespace OfficeTracker.Views.Pages;

/// <summary>
/// Represents a custom UserControl for the main window bar in the application.
/// It is responsible for displaying and managing the top bar of the main application window,
/// including UI elements such as layout, appearance, and interactions.
/// </summary>
public partial class MainPageBar : UserControl
{
	public MainPageBar()
	{
		InitializeComponent();
	}

	/// <summary>
	/// Handles the event to open the settings page. This method is invoked when
	/// the settings menu option is clicked in the main window.
	/// </summary>
	private void RequestOpenSettingsPage(object? sender, RoutedEventArgs e)
	{
		if (DataContext is not MainPageViewModel mpvm) return;
		mpvm.OpenSettings();
	}

	/// <summary>
	/// Handles the request to open the database save directory folder. This method is executed
	/// when the associated menu item is clicked in the main window's menu bar, triggering
	/// the logic within the current ViewModel.
	/// </summary>
	private void RequestOpenDatabaseSaveDirectoryFolder(object? sender, RoutedEventArgs e)
	{
		if (DataContext is not MainPageViewModel mpvm) return;
		mpvm.OpenDatabaseDirectoryFolder();
	}
}

