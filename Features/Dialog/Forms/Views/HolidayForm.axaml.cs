namespace OfficeTracker.Features.Dialog.Forms.Views;

public partial class HolidayForm : UserControl
{
	public HolidayForm()
	{
		InitializeComponent();
	}

	/// Gets the selected start date from the DatePicker control.
	/// This property retrieves the date selected in the start date picker (DatePickerStart).
	/// If a date is selected, it returns the date part (without time) as a nullable DateTime object.
	/// If no date is selected, it returns null.
	public DateTime? SelectedStartDate
	{
		get
		{
			DateTimeOffset? selectedDateTimeOffset = DatePickerStart.SelectedDate;
			if (selectedDateTimeOffset.HasValue)
				return selectedDateTimeOffset.Value.Date;
			else
				return null;
		}
	}

	/// Gets the selected end date from the DatePickerEnd control.
	/// This property retrieves the date selected in the end date picker (DatePickerEnd).
	/// If a date is selected, it returns the date part (without time) as a nullable DateTime object.
	/// If no date is selected, it returns null.
	public DateTime? SelectedEndDate
	{
		get
		{
			DateTimeOffset? selectedDateTimeOffset = DatePickerEnd.SelectedDate;
			if (selectedDateTimeOffset.HasValue)
				return selectedDateTimeOffset.Value.Date;
			else
				return null;
		}
	}
}

