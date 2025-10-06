namespace OfficeTracker.ViewModels.Pages;

public sealed partial class MainPageViewModel
{
	public void OpenSettings()
		=> DialogHelper.ShowDialogAsync("Achtung", "Dieses Feature ist noch nicht verfügbar!", DialogType.WARNING);
	public void OpenDatabaseDirectoryFolder()
		=> DialogHelper.ShowDialogAsync("Achtung", "Dieses Feature ist noch nicht verfügbar!", DialogType.WARNING);
}
