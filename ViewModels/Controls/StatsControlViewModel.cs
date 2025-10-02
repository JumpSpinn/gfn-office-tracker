namespace OfficeTracker.ViewModels.Controls;

using Services;

/// <summary>
/// Represents the view model for the StatsControl component in the application.
/// It manages the logic and state related to statistics calculations and display.
/// </summary>
[RegisterSingleton]
public sealed partial class StatsControlViewModel : ViewModelBase
{
	private readonly LogService _logService;

	public StatsControlViewModel(LogService lc)
	{
		_logService = lc;
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
        _logService.Debug("Stats calculated successfully.");
    }
}
