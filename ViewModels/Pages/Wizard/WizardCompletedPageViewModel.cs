namespace OfficeTracker.ViewModels.Pages.Wizard;

[RegisterSingleton]
public sealed partial class WizardCompletedPageViewModel : ViewModelBase
{
	private readonly LogController _logController;

	public WizardCompletedPageViewModel( LogController lc, WizardNamePageViewModel wnpvm, WizardDaysPageViewModel wdpvm, WizardDataPageViewModel wDpvm, WizardBalancePageViewModel wbpvm)
	{
		_logController = lc;
		WizardNamePageViewModel = wnpvm;
		WizardDaysPageViewModel = wdpvm;
		WizardDataPageViewModel = wDpvm;
		WizardBalancePageViewModel = wbpvm;
	}

	[ObservableProperty]
	private WizardNamePageViewModel _wizardNamePageViewModel;

	[ObservableProperty]
	private WizardDaysPageViewModel _wizardDaysPageViewModel;

	[ObservableProperty]
	private WizardDataPageViewModel _wizardDataPageViewModel;

	[ObservableProperty]
	private WizardBalancePageViewModel _wizardBalancePageViewModel;

	public string CurrentDayTracked
		=> WizardDataPageViewModel.CurrentDayTracked ? "Ja" : "Nein";

	[RelayCommand]
	private void NextSetupPage() => ChangePage(Page.MAIN);

	[RelayCommand]
	private void PreviousSetupPage() => ChangePage(Page.WIZARD_DATA);
}
