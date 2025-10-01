namespace OfficeTracker.ViewModels.Pages;

[RegisterSingleton]
public sealed partial class SetupNamePageViewModel : ViewModelBase
{
	private readonly LogController _logController;

	public SetupNamePageViewModel(LogController lc)
	{
		_logController = lc;
	}

	[ObservableProperty]
	private string _name = string.Empty;

	[RelayCommand]
	private void NextSetupPage()
	{
		ChangePage(Page.SETUP_BALANCE);
		_logController.Debug($"Name: {Name}");
	}
}
