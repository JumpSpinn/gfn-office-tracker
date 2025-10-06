namespace OfficeTracker.ViewModels.Windows;

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

	public void OpenSaveFolder()
		=> DialogHelper.ShowDialogAsync("Achtung", "Dieses Feature ist noch nicht verfügbar!", DialogType.WARNING);

	#endregion
}
