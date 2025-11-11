namespace OfficeTracker.Features.Dialog.Forms.ViewModels;

public sealed partial class DefaultHomeOfficeDaysFormViewModel : ViewModelBase
{
	/// <summary>
	/// A readonly dictionary mapping days of the week to functions that indicate whether each day
	/// is selected as a default home office day in the current view model state.
	/// </summary>
	private readonly Dictionary<DayOfWeek, Func<bool>> _daySelections;

	public DefaultHomeOfficeDaysFormViewModel()
	{
		_daySelections = new()
		{
			[DayOfWeek.Monday] = () => MondaySelected,
			[DayOfWeek.Tuesday] = () => TuesdaySelected,
			[DayOfWeek.Wednesday] = () => WednesdaySelected,
			[DayOfWeek.Thursday] = () => ThursdaySelected,
			[DayOfWeek.Friday] = () => FridaySelected
		};
	}

	#region DATA

	/// <summary>
	/// Sets the data for the default home office days by updating the internal state
	/// and marking the selected days accordingly.
	/// </summary>
	public void SetData(DayOfWeek[] homeOfficeDays)
	{
		var selectedDays = homeOfficeDays.ToHashSet();

		foreach (var day in Enum.GetValues<DayOfWeek>())
		{
			var isSelected = selectedDays.Contains(day);
			switch (day)
			{
				case DayOfWeek.Monday: MondaySelected = isSelected; break;
				case DayOfWeek.Tuesday: TuesdaySelected = isSelected; break;
				case DayOfWeek.Wednesday: WednesdaySelected = isSelected; break;
				case DayOfWeek.Thursday: ThursdaySelected = isSelected; break;
				case DayOfWeek.Friday: FridaySelected = isSelected; break;
			}
		}
	}

	/// <summary>
	/// Retrieves the selected and unselected home office days based on the internal state.
	/// </summary>
	public (DayOfWeek[] Selected, DayOfWeek[] Unselected) GetData()
	{
		var selected = _daySelections
			.Where(kvp => kvp.Value())
			.Select(kvp => kvp.Key)
			.ToArray();

		var unselected = _daySelections
			.Where(kvp => !kvp.Value())
			.Select(kvp => kvp.Key)
			.ToArray();

		return (selected, unselected);
	}

	#endregion


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
}
