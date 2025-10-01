namespace OfficeTracker.ViewModels.Pages;

[RegisterSingleton]
public sealed partial class SetupDaysPageViewModel : ViewModelBase
{
	private readonly LogController _logController;

	public SetupDaysPageViewModel(LogController lc)
	{
		_logController = lc;
	}

	[ObservableProperty]
	private bool _mondaySelected;

	[ObservableProperty]
	private bool _tuesdaySelected;

	[ObservableProperty]
	private bool _wednesdaySelected;

	[ObservableProperty]
	private bool _thursdaySelected;

	[ObservableProperty]
	private bool _fridaySelected;

	[ObservableProperty]
	private bool _saturdaySelected;

	[RelayCommand]
	private void NextSetupPage()
	{
		ChangePage(Page.SETUP_DATA);
		_logController.Debug($"Monday: {MondaySelected}, Monday: {TuesdaySelected}, Monday: {WednesdaySelected}, Monday: {ThursdaySelected}, Monday: {FridaySelected}, Monday: {SaturdaySelected}");
	}

	[RelayCommand]
	private void PreviousSetupPage() => ChangePage(Page.SETUP_BALANCE);
}
