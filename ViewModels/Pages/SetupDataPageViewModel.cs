namespace OfficeTracker.ViewModels.Pages;

[RegisterSingleton]
public sealed partial class SetupDataPageViewModel : ViewModelBase
{
	private readonly LogController _logController;

	public SetupDataPageViewModel(LogController lc)
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
		ChangePage(Page.SETUP_COMPLETED);
		_logController.Debug($"HomeOffice Days: {HomeOfficeDays}, Office Days: {OfficeDays}, Current day tracked? {CurrentDayTracked}");
	}

	[RelayCommand]
	private void PreviousSetupPage() => ChangePage(Page.SETUP_DAYS);
}
