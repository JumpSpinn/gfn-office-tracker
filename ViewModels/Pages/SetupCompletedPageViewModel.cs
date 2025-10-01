namespace OfficeTracker.ViewModels.Pages;

[RegisterSingleton]
public sealed partial class SetupCompletedPageViewModel : ViewModelBase
{
	public SetupCompletedPageViewModel()
	{

	}

	[RelayCommand]
	private void NextSetupPage() => ChangePage(Page.MAIN);

	[RelayCommand]
	private void PreviousSetupPage() => ChangePage(Page.SETUP_DATA);
}
