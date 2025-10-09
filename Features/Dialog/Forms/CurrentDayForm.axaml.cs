namespace OfficeTracker.Views.Forms;

/// <summary>
/// Represents the form for selecting the type of day (e.g., home office or office)
/// for the current day in the Office Tracker application.
/// </summary>
public sealed partial class CurrentDayForm : UserControl
{
    public CurrentDayForm()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Gets the type of the currently selected day in the current day form.
    /// Determines whether the selected day type is set as home office (DayType.HOME)
    /// or office (DayType.OFFICE). If no selection is made, returns null.
    /// </summary>
    public DayType? SelectedDayType
    {
	    get
	    {
		    if (HomeOfficeRadio.IsChecked == true)
			    return DayType.HOME;
		    else if (OfficeRadio.IsChecked == true)
			    return DayType.OFFICE;
		    else
			    return null;
	    }
    }
}
