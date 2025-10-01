namespace OfficeTracker.ViewModels.Pages;

[RegisterSingleton]
public sealed partial class SetupBalancePageViewModel : ViewModelBase
{
	public SetupBalancePageViewModel()
	{

	}

	[RelayCommand]
	public void NextSetupPage() => ChangePage(Page.SETUP_DAYS);

	[RelayCommand]
	public void PreviousSetupPage() => ChangePage(Page.SETUP_NAME);
}
