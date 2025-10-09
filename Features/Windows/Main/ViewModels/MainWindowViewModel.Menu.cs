namespace OfficeTracker.Features.Windows.Main.ViewModels;

/// <summary>
/// Represents the ViewModel for the main window of the application.
/// This ViewModel is responsible for managing the state, interactions,
/// and operations related to the main window, including handling menu actions,
/// page navigation, and other UI events.
/// </summary>
public sealed partial class MainWindowViewModel
{
	#region SETTINGS

	[ObservableProperty]
	private bool _settingsMenuOpened;

	public void ToggleSettingsMenu()
	{
		SettingsMenuOpened = !SettingsMenuOpened;
		ChangePage(SettingsMenuOpened ? Page.SETTINGS_WINDOW : Page.MAIN_WINDOW);
	}

	#endregion

	#region SAVE FOLDER

	/// <summary>
	/// Opens the folder where the application's save or database files are stored
	/// in the operating system's default file explorer. This method uses helper
	/// utilities to resolve the save folder path and facilitate folder navigation.
	/// </summary>
	public void OpenSaveFolder()
		=> ExplorerHelper.OpenFolder(_configController.Config.DatabasePath);

	#endregion
}
