namespace OfficeTracker.ViewModels.Controls;

using Base;

[RegisterSingleton]
public sealed partial class StatsControlViewModel : ViewModelBase
{
    [ObservableProperty]
    private uint _homeOfficeDays = 13;

    [ObservableProperty]
    private double _homeOfficePercent = 50;

    [ObservableProperty]
    private uint _officeDays = 13;

    [ObservableProperty]
    private double _officePercent = 50;

    public StatsControlViewModel()
    {
        //_homeOfficeDays = 35;
        //_officeDays = 27;
        CalculateStats();
    }

    private void CalculateStats()
    {
        var totalDays = HomeOfficeDays + OfficeDays;

        HomeOfficePercent = (double)HomeOfficeDays / totalDays;
        OfficePercent = (double)OfficeDays / totalDays;

        // var total = homeOfficePercent + officePercent;
        // if (Math.Abs(total - 100) > 0)
        // {
        //     homeOfficePercent = (homeOfficePercent / total) * 100;
        //     officePercent = (officePercent / total) * 100;
        // }
        //
        // StatsGrid.ColumnDefinitions[0] = new ColumnDefinition(new GridLength(homeOfficePercent, GridUnitType.Star));
        // StatsGrid.ColumnDefinitions[1] = new ColumnDefinition(new GridLength(officePercent, GridUnitType.Star));
        //
        // StatsLabel.IsVisible = false;
        // HomeOfficeText.Text = $"{homeOfficePercent:F2}%";
        // StandortText.Text = $"{officePercent:F2}%";
        //
        // if (homeOfficePercent > 50.00)
        // {
        //     StatsLabel.Text = "Deine Quote ist kacke";
        //     StatsLabel.Foreground = Brush.Parse("#d08b8b");
        //     StatsLabel.IsVisible = true;
        // }
    }
}
