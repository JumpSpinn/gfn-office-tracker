namespace OfficeTracker.Views.Forms;

public partial class PlannableDayForm : UserControl
{
    public PlannableDayForm()
    {
        InitializeComponent();
    }

    public DateTime GetSelectedDate
    {
	    get
	    {
		    DateTimeOffset? selectedDateTimeOffset = DatePicker.SelectedDate;
		    if (selectedDateTimeOffset.HasValue)
			    return selectedDateTimeOffset.Value.Date;
		    else
			    return DateTime.Today.AddDays(-1);
	    }
    }

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
