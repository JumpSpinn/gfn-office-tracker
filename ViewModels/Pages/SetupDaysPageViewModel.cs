namespace OfficeTracker.ViewModels.Pages;

[RegisterSingleton]
public sealed partial class SetupDaysPageViewModel : ViewModelBase
{
	public SetupDaysPageViewModel()
	{

	}

	[RelayCommand]
	private void NextSetupPage() => ChangePage(Page.SETUP_DATA);

	[RelayCommand]
	private void PreviousSetupPage() => ChangePage(Page.SETUP_BALANCE);
}
