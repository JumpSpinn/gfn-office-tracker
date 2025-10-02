namespace OfficeTracker.ViewModels.Pages.Wizard;

/// <summary>
/// Represents the ViewModel used for handling and managing the logic
/// of wizard days selection in the wizard-based workflow.
/// </summary>
[RegisterSingleton]
public sealed partial class WizardDaysPageViewModel : ViewModelBase
{
	/// <summary>
	/// Gets a value indicating whether any day has been selected in the wizard days selection process.
	/// </summary>
	public bool IsDaySelected =>
		MondaySelected || TuesdaySelected || WednesdaySelected ||
		ThursdaySelected || FridaySelected || SaturdaySelected;

	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(IsDaySelected))]
	private bool _mondaySelected;

	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(IsDaySelected))]
	private bool _tuesdaySelected;

	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(IsDaySelected))]
	private bool _wednesdaySelected;

	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(IsDaySelected))]
	private bool _thursdaySelected;

	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(IsDaySelected))]
	private bool _fridaySelected;

	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(IsDaySelected))]
	private bool _saturdaySelected;

	/// <summary>
	/// Gets the string representation of the selected days in the wizard workflow.
	/// </summary>
	public string SelectedDays { get; private set; } = string.Empty;

	/// <summary>
	/// Converts the boolean values indicating selected days into a string representation.
	/// This method processes a set of boolean flags for each day of the week, representing
	/// whether a specific day is selected, and maps these flags to their respective
	/// abbreviated forms. The resulting string contains the selected days concatenated,
	/// separated by a comma.
	/// </summary>
	private void ConvertSelectedBooleanDays()
	{
		var days = new[]
		{
			(MondaySelected, "Mo"),
			(TuesdaySelected, "Di"),
			(WednesdaySelected, "Mi"),
			(ThursdaySelected, "Do"),
			(FridaySelected, "Fr"),
			(SaturdaySelected, "Sa")
		};
		SelectedDays = string.Join(", ", days
			.Where(d => d.Item1)
			.Select(d => d.Item2));
	}

	/// <summary>
	/// Navigates to the next setup page in the wizard-based workflow if a day is selected.
	/// This method checks whether at least one day has been marked as selected. If so, it converts
	/// the selected days into a specific format and transitions the user to the data setup page.
	/// </summary>
	[RelayCommand]
	private void NextSetupPage()
	{
		if (!IsDaySelected) return;
		ConvertSelectedBooleanDays();
		ChangePage(Page.WIZARD_DATA);
	}

	[RelayCommand]
	private void PreviousSetupPage() => ChangePage(Page.WIZARD_BALANCE);
}
