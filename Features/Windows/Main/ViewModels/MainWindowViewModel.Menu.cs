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

	/// <summary>
	/// Indicates whether the settings menu is currently opened.
	/// This property is used to track and manage the visibility state
	/// of the settings menu in the main window's ViewModel.
	/// </summary>
	[ObservableProperty]
	private bool _settingsMenuOpened;

	/// <summary>
	/// Toggles the visibility of the settings menu in the main window. When the
	/// settings menu is opened, the application navigates to the settings page.
	/// Conversely, closing the settings menu navigates back to the main window page.
	/// This method is used to control the state of the settings menu and its related
	/// page transitions.
	/// </summary>
	[RelayCommand]
	private void ToggleSettingsMenu()
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
	[RelayCommand]
	private void OpenSaveFolder()
		=> ExplorerHelper.OpenFolder(_configController.ConfigEntity.DatabasePath);

	#endregion
}
