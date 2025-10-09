namespace OfficeTracker.Features.Screens.Wizard.ViewModels;

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
		ThursdaySelected || FridaySelected;

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

	/// <summary>
	/// Gets the string representation of the selected days in the wizard workflow.
	/// </summary>
	public string SelectedDaysDisplay { get; private set; }
		= string.Empty;

	/// <summary>
	/// Gets a string representation of the days that have not been selected
	/// in the wizard days selection process.
	/// </summary>
	public string UnselectedDaysDisplay { get; private set; }
		= string.Empty;

	/// <summary>
	/// Gets an array of days that have been selected in the wizard days selection process.
	/// </summary>
	public DayOfWeek[] SelectedDayEnums { get; private set; }
		= [];

	/// <summary>
	/// Gets an array of <see cref="DayOfWeek"/> values representing the days
	/// that have not been selected in the wizard days selection process.
	/// </summary>
	public DayOfWeek[] UnselectedDayEnums { get; private set; }
		= [];

	/// <summary>
	/// Converts the currently selected days and unselected days into corresponding
	/// string representations and enumerations. This method processes the values
	/// of the boolean properties representing each day of the week and maps them
	/// to their respective display formats and DayOfWeek enum values. The result
	/// is stored in properties that represent the selected and unselected days
	/// in both displayable and enumerated formats.
	/// </summary>
	private void ConvertSelectedBooleanDays()
	{
		var baseMappings = new List<(bool IsSelected, string Abbreviation, DayOfWeek Day)>
		{
			(MondaySelected, "Mo", DayOfWeek.Monday),
			(TuesdaySelected, "Di", DayOfWeek.Tuesday),
			(WednesdaySelected, "Mi", DayOfWeek.Wednesday),
			(ThursdaySelected, "Do", DayOfWeek.Thursday),
			(FridaySelected, "Fr", DayOfWeek.Friday)
		};

		SelectedDaysDisplay = string.Join(", ", baseMappings
			.Where(d => d.Item1)
			.Select(d => d.Item2)
			.ToArray());

		UnselectedDaysDisplay = string.Join(", ", baseMappings
			.Where(d => !d.Item1)
			.Select(d => d.Item2)
			.ToArray());

		SelectedDayEnums = baseMappings
			.Where(d => d.Item1)
			.Select(d => d.Item3)
			.ToArray();

		UnselectedDayEnums = baseMappings
			.Where(d => !d.Item1)
			.Select(d => d.Item3)
			.ToArray();
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
	private void PreviousSetupPage()
		=> ChangePage(Page.WIZARD_BALANCE);
}
