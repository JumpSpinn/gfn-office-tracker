namespace OfficeTracker.ViewModels.Controls;

/// <summary>
/// Represents the view model for the StatsControl component in the application.
/// It manages the logic and state related to statistics calculations and display.
/// </summary>
[RegisterSingleton]
public sealed partial class StatsControlViewModel : ViewModelBase
{
	private readonly LogController _logController;

	public StatsControlViewModel(LogController lc)
	{
		_logController = lc;
		CalculateStats();
	}

    [ObservableProperty]
    private uint _homeOfficeDays = 13;

    [ObservableProperty]
    private double _homeOfficePercent = 50;

    [ObservableProperty]
    private uint _officeDays = 13;

    [ObservableProperty]
    private double _officePercent = 50;

    private void CalculateStats()
    {
        var totalDays = HomeOfficeDays + OfficeDays;
        HomeOfficePercent = (double)HomeOfficeDays / totalDays;
        OfficePercent = (double)OfficeDays / totalDays;
        _logController.Debug("Stats calculated successfully.");
    }
}
