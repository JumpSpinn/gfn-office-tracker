namespace OfficeTracker.Views.Forms;

/// <summary>
/// Represents a user control form for planning a specific day type and selecting a specific date.
/// </summary>
public partial class PlannableDayForm : UserControl
{
    public PlannableDayForm()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Gets the selected date from the associated date picker control.
    /// </summary>
    public DateTime? SelectedDate
    {
	    get
	    {
		    DateTimeOffset? selectedDateTimeOffset = DatePicker.SelectedDate;
		    if (selectedDateTimeOffset.HasValue)
			    return selectedDateTimeOffset.Value.Date;
		    else
			    return null;
	    }
    }

    /// <summary>
    /// Gets the selected day type, representing whether the planned day is a home office day,
    /// an office day, or if no specific type is selected.
    /// </summary>
    public DayType SelectedDayType
    {
	    get
	    {
		    if (HomeOfficeRadio.IsChecked == true)
			    return DayType.HOME;
		    else if (OfficeRadio.IsChecked == true)
			    return DayType.OFFICE;
		    else
			    return DayType.NONE;
	    }
    }
}
