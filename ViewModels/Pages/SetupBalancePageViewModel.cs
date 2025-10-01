namespace OfficeTracker.ViewModels.Pages;

[RegisterSingleton]
public sealed partial class SetupBalancePageViewModel : ViewModelBase
{
	public SetupBalancePageViewModel()
	{

	}

	[RelayCommand]
	private void NextSetupPage() => ChangePage(Page.SETUP_DAYS);

	[RelayCommand]
	private void PreviousSetupPage() => ChangePage(Page.SETUP_NAME);
}
