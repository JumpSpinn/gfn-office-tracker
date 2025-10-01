namespace OfficeTracker.ViewModels.Pages;

[RegisterSingleton]
public sealed partial class SetupWelcomePageViewModel : ViewModelBase
{
	public SetupWelcomePageViewModel()
	{

	}

	[RelayCommand]
	public void NextSetupPage() => ChangePage(Page.SETUP_NAME);
}
