namespace OfficeTracker.ViewModels.Pages;

[RegisterSingleton]
public sealed partial class SetupWelcomePageViewModel : ViewModelBase
{
	public SetupWelcomePageViewModel()
	{

	}

	[RelayCommand]
	private void NextSetupPage() => ChangePage(Page.SETUP_NAME);
}
