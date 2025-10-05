namespace OfficeTracker.Views.Pages;

/// <summary>
/// Represents the header section of the main page in the application.
/// This UserControl is responsible for displaying and managing the UI
/// elements associated with the header of the main page.
/// </summary>
public partial class MainPageHeader : UserControl
{
	public MainPageHeader()
	{
		InitializeComponent();
	}

	/// <summary>
	/// Handles the AddButtonClicked event from the StatisticControl. This method triggers the
	/// execution of an asynchronous operation to display a dialog for adding the current day's data.
	/// </summary>
	private void RequestShowAddCurrentDayDialog(object? sender, RoutedEventArgs e)
	{
		if(DataContext is not MainPageViewModel mpv) return;
		mpv.ShowAddCurrentDayDialogAsync();
	}
}

