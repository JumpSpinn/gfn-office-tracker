namespace OfficeTracker.ViewModels.Pages;

[RegisterSingleton]
public sealed partial class SetupNamePageViewModel : ViewModelBase
{
	public SetupNamePageViewModel()
	{

	}

	[RelayCommand]
	public void NextSetupPage() => ChangePage(Page.SETUP_OPTION);
}
