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

	/// <summary>
	/// Represents the total number of days spent working from home, as tracked within the wizard process.
	/// Provides a numeric value retrieved from the underlying <see cref="WizardDataPageViewModel"/>.
	/// If no value is available, it defaults to zero.
	/// </summary>
	public decimal HomeOfficeDays
		=> WizardDataPageViewModel.HomeOfficeDays ?? 0;

	/// <summary>
	/// Represents the number of office days calculated or stored in the "Wizard Completed" process.
	/// Retrieves its value from the <see cref="WizardDataPageViewModel"/>, defaulting to 0 if undefined.
	/// </summary>
	public decimal OfficeDays
		=> WizardDataPageViewModel.OfficeDays ?? 0;

	[RelayCommand]
	private async Task NextSetupPage()
	{
		EnableBlurEffect();

		var dialogResult = await DialogHelper.ShowDialogAsync(
			"Setup-Assistent",
			"Hast du alle Daten auf Richtigkeit geprüft? Du kannst danach keine Änderungen mehr vornehmen!",
			DialogType.QUESTION,
			"Alles korrekt!",
			"Abbrechen"
			);

		DisableBlurEffect();

		if(dialogResult == ContentDialogResult.Primary)
			ChangePage(Page.MAIN);
	}

	[RelayCommand]
	private void PreviousSetupPage() => ChangePage(Page.WIZARD_DATA);
}
