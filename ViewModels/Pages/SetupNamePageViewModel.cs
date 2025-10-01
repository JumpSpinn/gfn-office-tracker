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
	public void NextSetupPage() => ChangePage(Page.SETUP_BALANCE);
}
