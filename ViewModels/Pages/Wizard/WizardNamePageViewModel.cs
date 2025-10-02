namespace OfficeTracker.ViewModels.Pages.Wizard;

/// <summary>
/// Represents the ViewModel for the Wizard Name Page in the application's wizard flow.
/// </summary>
[RegisterSingleton]
public sealed partial class WizardNamePageViewModel : ViewModelBase
{
	[ObservableProperty]
	private string _name = string.Empty;

	[RelayCommand]
	private void NextSetupPage() => ChangePage(Page.WIZARD_BALANCE);
}
