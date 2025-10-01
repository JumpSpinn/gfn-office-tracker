namespace OfficeTracker.ViewModels.Pages;

[RegisterSingleton]
public sealed partial class SetupDaysPageViewModel : ViewModelBase
{
	public SetupDaysPageViewModel()
	{

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
	private void NextSetupPage() => ChangePage(Page.SETUP_DATA);

	[RelayCommand]
	private void PreviousSetupPage() => ChangePage(Page.SETUP_BALANCE);
}
