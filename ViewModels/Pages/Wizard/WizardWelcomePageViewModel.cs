namespace OfficeTracker.ViewModels.Pages.Wizard;

[RegisterSingleton]
public sealed partial class WizardWelcomePageViewModel : ViewModelBase
{
	public WizardWelcomePageViewModel()
	{

	}

	[RelayCommand]
	private void NextSetupPage() => ChangePage(Page.WIZARD_NAME);
}
