namespace OfficeTracker.ViewModels.Pages;

[RegisterSingleton]
public sealed partial class SetupOptionPageViewModel : ViewModelBase
{
	public SetupOptionPageViewModel()
	{

	}

	[RelayCommand]
	public void NextSetupPage() => ChangePage(Page.SETUP_DAYS);

	[RelayCommand]
	public void PreviousSetupPage() => ChangePage(Page.SETUP_NAME);
}
