namespace OfficeTracker.ViewModels.Pages.Wizard;

/// <summary>
/// Represents the view model for the "Wizard Completed" page in the wizard process.
/// </summary>
[RegisterSingleton]
public sealed partial class WizardCompletedPageViewModel : ViewModelBase
{
	public WizardCompletedPageViewModel(WizardNamePageViewModel name, WizardDaysPageViewModel days, WizardDataPageViewModel data, WizardBalancePageViewModel balance)
	{
		WizardNamePageViewModel = name;
		WizardDaysPageViewModel = days;
		WizardDataPageViewModel = data;
		WizardBalancePageViewModel = balance;
	}

	[ObservableProperty]
	private WizardNamePageViewModel _wizardNamePageViewModel;

	[ObservableProperty]
	private WizardDaysPageViewModel _wizardDaysPageViewModel;

	[ObservableProperty]
	private WizardDataPageViewModel _wizardDataPageViewModel;

	[ObservableProperty]
	private WizardBalancePageViewModel _wizardBalancePageViewModel;

	/// <summary>
	/// Represents the current day being tracked in the wizard process.
	/// Displays a localized string value ("Ja" or "Nein") indicating the tracking status of the current day.
	/// The value is derived from the <see cref="WizardDataPageViewModel"/>.
	/// </summary>
	public string CurrentDayTracked
		=> WizardDataPageViewModel.CurrentDayTracked ? "Ja" : "Nein";

	[RelayCommand]
	private void NextSetupPage() => ChangePage(Page.MAIN);

	[RelayCommand]
	private void PreviousSetupPage() => ChangePage(Page.WIZARD_DATA);
}
