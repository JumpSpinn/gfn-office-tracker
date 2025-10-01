namespace OfficeTracker.ViewModels.Pages.Wizard;

[RegisterSingleton]
public sealed partial class WizardNamePageViewModel : ViewModelBase
{
	private readonly LogController _logController;

	public WizardNamePageViewModel(LogController lc)
	{
		_logController = lc;
	}

	[ObservableProperty]
	private string _name = string.Empty;

	[RelayCommand]
	private void NextSetupPage()
	{
		ChangePage(Page.WIZARD_BALANCE);
		_logController.Debug($"Name: {Name}");
	}
}
