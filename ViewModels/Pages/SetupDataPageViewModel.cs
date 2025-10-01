namespace OfficeTracker.ViewModels.Pages;

[RegisterSingleton]
public sealed partial class SetupDataPageViewModel : ViewModelBase
{
	public SetupDataPageViewModel()
	{

	}

	[ObservableProperty]
	private decimal _homeOfficeDays;

	[ObservableProperty]
	private decimal _OfficeDays;

	[ObservableProperty]
	private bool _currentDayTracked;

	[RelayCommand]
	private void NextSetupPage() => ChangePage(Page.SETUP_COMPLETED);

	[RelayCommand]
	private void PreviousSetupPage() => ChangePage(Page.SETUP_DAYS);
}
