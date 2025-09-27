namespace OfficeTracker.Views.Controls;

public sealed class StatsControl : TemplatedControl
{
	public StatsControl(bool asList = false)
	{
		if (asList)
		{
			Margin = new Thickness(0, 10, 0, 10);
		}
	}

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        _grid = e.NameScope.Find<Grid>("StatsGrid");
        _officePercentageText = e.NameScope.Find<TextBlock>("OfficePercentage");
        _homeOfficePercentageText = e.NameScope.Find<TextBlock>("HomeOfficePercentage");
        _homeStackPanel = e.NameScope.Find<StackPanel>("HomeStackPanel");
        _officeStackPanel = e.NameScope.Find<StackPanel>("OfficeStackPanel");
        _error = e.NameScope.Find<Border>("Error");
        _warning = e.NameScope.Find<Border>("Warning");

        base.OnApplyTemplate(e);
        CalculateStats();
    }

    #region HOMEOFFICE
    private StackPanel? _homeStackPanel;
    private TextBlock? _homeOfficePercentageText;

    private static readonly StyledProperty<uint> _homeOfficeDaysProperty =
        AvaloniaProperty.Register<StatsControl, uint>(nameof(HomeOfficeDays));

    public uint HomeOfficeDays
    {
        get => GetValue(_homeOfficeDaysProperty);
        init => SetValue(_homeOfficeDaysProperty, value);
    }
    #endregion

    #region OFFICE
    private StackPanel? _officeStackPanel;
    private TextBlock? _officePercentageText;

    private static readonly StyledProperty<uint> _officeDaysProperty =
        AvaloniaProperty.Register<StatsControl, uint>(nameof(OfficeDays));

    public uint OfficeDays
    {
        get => GetValue(_officeDaysProperty);
        init => SetValue(_officeDaysProperty, value);
    }

    #endregion

    #region CALCULATION

    private Grid? _grid;
    private Border? _warning;
    private Border? _error;

    private void CalculateStats()
    {
        if (_grid is null) return;
        if (_homeOfficePercentageText is null) return;
        if (_officePercentageText is null) return;
        if (_homeStackPanel is null) return;
        if (_officeStackPanel is null) return;
        if (_warning is null) return;
        if (_error is null) return;

        var totalDays = HomeOfficeDays + OfficeDays;
        var homeOfficePercentage = (double)HomeOfficeDays / totalDays * 100;
        var officePercentage = (double)OfficeDays / totalDays * 100;

        _grid.ColumnDefinitions[0].Width = new GridLength(homeOfficePercentage, GridUnitType.Star);
        _grid.ColumnDefinitions[1].Width = new GridLength(officePercentage, GridUnitType.Star);

        _homeOfficePercentageText.Text = $"{homeOfficePercentage:F2}%";
        _officePercentageText.Text = $"{officePercentage:F2}%";

        _homeStackPanel.SetValue(ToolTip.TipProperty, $"{HomeOfficeDays} Tage");
        _officeStackPanel.SetValue(ToolTip.TipProperty, $"{OfficeDays} Tage");

        _error.IsVisible = homeOfficePercentage > 50.00;
        _warning.IsVisible = homeOfficePercentage == 50.00;
    }

    #endregion
}
