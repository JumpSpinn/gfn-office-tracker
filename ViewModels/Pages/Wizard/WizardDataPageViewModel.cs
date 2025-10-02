namespace OfficeTracker.ViewModels.Pages.Wizard;

/// <summary>
/// Represents the view model for the data page of the wizard process.
/// </summary>
[RegisterSingleton]
public sealed partial class WizardDataPageViewModel : ViewModelBase
{
	[ObservableProperty]
	private decimal _homeOfficeDays;

	[ObservableProperty]
	private decimal _officeDays;

	[ObservableProperty]
	private bool _currentDayTracked;

	[RelayCommand]
	private void NextSetupPage() => ChangePage(Page.WIZARD_COMPLETED);

	[RelayCommand]
	private void PreviousSetupPage() => ChangePage(Page.WIZARD_DAYS);
}
