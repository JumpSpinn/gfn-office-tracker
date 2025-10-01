namespace OfficeTracker.ViewModels.Pages;

[RegisterSingleton]
public sealed partial class SetupDataPageViewModel : ViewModelBase
{
	public SetupDataPageViewModel()
	{

	}

	[RelayCommand]
	public void NextSetupPage() => ChangePage(Page.SETUP_COMPLETED);

	[RelayCommand]
	public void PreviousSetupPage() => ChangePage(Page.SETUP_DAYS);
}
