namespace OfficeTracker.ViewModels.Pages;

[RegisterSingleton]
public sealed partial class SetupNamePageViewModel : ViewModelBase
{
	public SetupNamePageViewModel()
	{

	}

	[ObservableProperty]
	private string _name = string.Empty;

	[RelayCommand]
	private void NextSetupPage() => ChangePage(Page.SETUP_BALANCE);
}
