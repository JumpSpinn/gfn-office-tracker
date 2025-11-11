namespace OfficeTracker.Features.Screens.Wizard.ViewModels;

/// <summary>
/// Represents the ViewModel for the Wizard Name Page in the application's wizard flow.
/// </summary>
[RegisterSingleton]
public sealed partial class WizardNamePageViewModel : ViewModelBase
{
	[ObservableProperty]
	private string _name = string.Empty;

	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(IsValid))]
	private string _nameError = string.Empty;

	public bool IsValid => NameError == string.Empty;

	[RelayCommand]
	private void NextSetupPage()
	{
		var validation = StringHelper.ValidateUserName(Name);
		if (!validation.Result)
		{
			NameError = validation.Message;
			return;
		}
		ChangePage(Page.WIZARD_BALANCE);
	}
}
