namespace OfficeTracker.ViewModels.Pages;

[RegisterSingleton]
public sealed partial class SetupCompletedPageViewModel : ViewModelBase
{
	public SetupCompletedPageViewModel()
	{

	}

	[RelayCommand]
	public void NextSetupPage() => ChangePage(Page.MAIN);

	[RelayCommand]
	public void PreviousSetupPage() => ChangePage(Page.SETUP_DATA);
}
