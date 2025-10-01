namespace OfficeTracker.ViewModels.Pages.Wizard;

[RegisterSingleton]
public sealed partial class WizardDataPageViewModel : ViewModelBase
{
	private readonly LogController _logController;

	public WizardDataPageViewModel(LogController lc)
	{
		_logController = lc;
	}

	[ObservableProperty]
	private decimal _homeOfficeDays;

	[ObservableProperty]
	private decimal _officeDays;

	[ObservableProperty]
	private bool _currentDayTracked;

	[RelayCommand]
	private void NextSetupPage()
	{
		ChangePage(Page.WIZARD_COMPLETED);
		_logController.Debug($"HomeOffice Days: {HomeOfficeDays}, Office Days: {OfficeDays}, Current day tracked? {CurrentDayTracked}");
	}

	[RelayCommand]
	private void PreviousSetupPage() => ChangePage(Page.WIZARD_DAYS);
}
