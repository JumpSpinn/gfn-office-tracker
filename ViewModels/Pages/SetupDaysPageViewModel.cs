namespace OfficeTracker.ViewModels.Pages;

[RegisterSingleton]
public sealed partial class SetupDaysPageViewModel : ViewModelBase
{
	public SetupDaysPageViewModel()
	{

	}

	[RelayCommand]
	public void NextSetupPage() => ChangePage(Page.SETUP_DATA);

	[RelayCommand]
	public void PreviousSetupPage() => ChangePage(Page.SETUP_DAYS);
}
