namespace OfficeTracker.ViewModels.Pages.Wizard;

[RegisterSingleton]
public sealed partial class WizardCompletedPageViewModel : ViewModelBase
{
	private readonly LogController _logController;
	private readonly WizardNamePageViewModel _wizardNamePageViewModel;
	private readonly WizardDaysPageViewModel _wizardDaysPageViewModel;
	private readonly WizardDataPageViewModel _wizardDataPageViewModel;
	private readonly WizardBalancePageViewModel _wizardBalancePageViewModel;

	public WizardCompletedPageViewModel( LogController lc, WizardNamePageViewModel wnpvm, WizardDaysPageViewModel wdpvm, WizardDataPageViewModel wDpvm, WizardBalancePageViewModel wbpvm)
	{
		_logController = lc;
		_wizardNamePageViewModel = wnpvm;
		_wizardDaysPageViewModel = wdpvm;
		_wizardDataPageViewModel = wDpvm;
		_wizardBalancePageViewModel = wbpvm;
	}

	[RelayCommand]
	private void NextSetupPage() => ChangePage(Page.MAIN);

	[RelayCommand]
	private void PreviousSetupPage() => ChangePage(Page.WIZARD_DATA);
}
