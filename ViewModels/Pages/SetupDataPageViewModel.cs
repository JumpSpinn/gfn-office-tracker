namespace OfficeTracker.ViewModels.Pages;

[RegisterSingleton]
public sealed partial class SetupDataPageViewModel : ViewModelBase
{
	public SetupDataPageViewModel()
	{

	}

	[RelayCommand]
	private void NextSetupPage() => ChangePage(Page.SETUP_COMPLETED);

	[RelayCommand]
	private void PreviousSetupPage() => ChangePage(Page.SETUP_DAYS);
}
