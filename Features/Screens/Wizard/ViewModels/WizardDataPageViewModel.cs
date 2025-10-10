namespace OfficeTracker.Features.Screens.Wizard.ViewModels;

/// <summary>
/// Represents the view model for the data page of the wizard process.
/// </summary>
[RegisterSingleton]
public sealed partial class WizardDataPageViewModel : ViewModelBase
{
	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(CanTrackCurrentDay))]
	private decimal? _homeOfficeDays;

	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(CanTrackCurrentDay))]
	private decimal? _officeDays;

	[ObservableProperty]
	private bool _currentDayTracked;

	/// <summary>
	/// Gets a value indicating whether the current day can be tracked based on the specified home office and office day values.
	/// </summary>
	public bool CanTrackCurrentDay
		=> HomeOfficeDays > 0 || OfficeDays > 0;

	/// <summary>
	/// Invoked when the home office days value is changed.
	/// </summary>
	partial void OnHomeOfficeDaysChanged(decimal? value)
	{
		if (value is null || (value == 0 && OfficeDays == 0))
			CurrentDayTracked = false;
	}

	/// <summary>
	/// Invoked when the office days value is changed.
	/// </summary>
	partial void OnOfficeDaysChanged(decimal? value)
	{
		if (value is null || (value == 0 && HomeOfficeDays == 0))
			CurrentDayTracked = false;
	}

	[RelayCommand]
	private void NextSetupPage()
		=> ChangePage(Page.WIZARD_COMPLETED);

	[RelayCommand]
	private void PreviousSetupPage()
		=> ChangePage(Page.WIZARD_DAYS);
}
