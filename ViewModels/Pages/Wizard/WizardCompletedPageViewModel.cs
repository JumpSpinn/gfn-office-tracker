namespace OfficeTracker.ViewModels.Pages.Wizard;

[RegisterSingleton]
public sealed partial class WizardCompletedPageViewModel : ViewModelBase
{
	public WizardCompletedPageViewModel()
	{

	}

	[RelayCommand]
	private void NextSetupPage() => ChangePage(Page.MAIN);

	[RelayCommand]
	private void PreviousSetupPage() => ChangePage(Page.WIZARD_DATA);
}
