namespace OfficeTracker.ViewModels.Pages.Wizard;

/// <summary>
/// Represents the view model class for the wizard's welcome page in the application.
/// This class manages the state and behavior specific to the wizard's welcome page.
/// </summary>
[RegisterSingleton]
public sealed partial class WizardWelcomePageViewModel : ViewModelBase
{
	[RelayCommand]
	private void NextSetupPage() => ChangePage(Page.WIZARD_NAME);
}
