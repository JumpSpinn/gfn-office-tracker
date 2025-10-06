namespace OfficeTracker.ViewModels.Pages;

public sealed partial class MainPageViewModel
{
	/// <summary>
	/// Navigates to the settings page within the application.
	/// </summary>
	public void OpenSettings()
		=> ChangePage(Page.SETTINGS_WINDOW);

	public void OpenDatabaseDirectoryFolder()
		=> DialogHelper.ShowDialogAsync("Achtung", "Dieses Feature ist noch nicht verfügbar!", DialogType.WARNING);
}
