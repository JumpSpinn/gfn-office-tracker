namespace OfficeTracker.ViewModels.Pages.Settings;

[RegisterSingleton]
public sealed partial class SettingsPageViewModel : ViewModelBase
{
	[RelayCommand]
	private void LeaveSettingsPage()
		=> ChangePage(Page.MAIN_WINDOW);
}
