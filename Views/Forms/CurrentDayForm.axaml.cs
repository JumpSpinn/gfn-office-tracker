namespace OfficeTracker.Views.Forms;

public partial class CurrentDayForm : UserControl
{
    public CurrentDayForm()
    {
        InitializeComponent();
    }

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
