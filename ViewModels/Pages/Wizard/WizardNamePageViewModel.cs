namespace OfficeTracker.ViewModels.Pages.Wizard;

using System.Text.RegularExpressions;

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

	[GeneratedRegex(@"^[\p{L}\s\-']+$")]
	private static partial Regex NameRegex();

	private bool ValidateName()
	{
		NameError = string.Empty;
		if(string.IsNullOrEmpty(Name))
			NameError = "Name darf nicht leer sein.";
		else if(Name.Length > 32)
			NameError = "Name darf nur maximal 32 Zeichen lang sein.";
		else if(!NameRegex().IsMatch(Name.Trim()))
			NameError = "Name darf nur Buchstaben, Leerzeichen und Bindestriche enthalten.";
		return NameError == string.Empty;
	}

	[RelayCommand]
	private void NextSetupPage()
	{
		if (!ValidateName()) return;
		ChangePage(Page.WIZARD_BALANCE);
	}
}
