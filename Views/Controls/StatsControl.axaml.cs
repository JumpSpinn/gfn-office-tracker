namespace OfficeTracker.Views.Controls;

/// <summary>
/// Represents a control used for displaying statistics related to office and home office days.
/// </summary>
public sealed class StatsControl : TemplatedControl
{
	public StatsControl(bool asList = false)
	{
		if (asList)
			Margin = new Thickness(0, 10, 0, 10);
	}

	/// <summary>
	/// Invoked whenever the control's template is applied.
	/// This method is responsible for initializing the control's template-defined elements
	/// and performing setup operations after the control's visual representation is fully loaded.
	/// </summary>
	protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        _grid = e.NameScope.Find<Grid>("StatsGrid");
        _officePercentageText = e.NameScope.Find<TextBlock>("OfficePercentage");
        _homeOfficePercentageText = e.NameScope.Find<TextBlock>("HomeOfficePercentage");
        _homeStackPanel = e.NameScope.Find<StackPanel>("HomeStackPanel");
        _officeStackPanel = e.NameScope.Find<StackPanel>("OfficeStackPanel");
        _error = e.NameScope.Find<Border>("Error");
        _warning = e.NameScope.Find<Border>("Warning");

        CalculateStats();
        base.OnApplyTemplate(e);
    }

    #region HOMEOFFICE
    private StackPanel? _homeStackPanel;
    private TextBlock? _homeOfficePercentageText;

    /// <summary>
    /// A styled property that represents the number of home office days.
    /// This property is used to calculate statistics and update the visual representation
    /// of home office-related data in the <see cref="StatsControl"/>.
    /// </summary>
    private static readonly StyledProperty<uint> _homeOfficeDaysProperty =
        AvaloniaProperty.Register<StatsControl, uint>(nameof(HomeOfficeDays));

    public uint HomeOfficeDays
    {
        get => GetValue(_homeOfficeDaysProperty);
        init => SetValue(_homeOfficeDaysProperty, value);
    }

    /// <summary>
    /// Represents the percentage of home office days compared to the total number of days.
    /// The value is calculated as a ratio of <see cref="HomeOfficeDays"/> to the sum of
    /// <see cref="HomeOfficeDays"/> and <see cref="OfficeDays"/> and is used to update
    /// the graphical representation and statistical data in the <see cref="StatsControl"/>.
    /// </summary>
    public double HomeOfficePercentage
	    => (double)HomeOfficeDays / (HomeOfficeDays + OfficeDays) * 100;

    #endregion

    #region OFFICE
    private StackPanel? _officeStackPanel;
    private TextBlock? _officePercentageText;

    /// <summary>
    /// A styled property that represents the number of office days.
    /// This property is used to calculate statistics and update the visual representation
    /// of office-related data in the <see cref="StatsControl"/>.
    /// </summary>
    private static readonly StyledProperty<uint> _officeDaysProperty =
        AvaloniaProperty.Register<StatsControl, uint>(nameof(OfficeDays));

    public uint OfficeDays
    {
        get => GetValue(_officeDaysProperty);
        init => SetValue(_officeDaysProperty, value);
    }

    /// <summary>
    /// A calculated property that represents the percentage of office days relative to the total number of days.
    /// The value is derived by dividing the number of office days by the sum of office and home office days.
    /// This property is used to update and visually display office day statistics in the <see cref="StatsControl"/>.
    /// </summary>
    public double OfficePercentage
	    => (double)OfficeDays / (HomeOfficeDays + OfficeDays) * 100;

    #endregion

    #region CALCULATION

    private Grid? _grid;
    private Border? _warning;
    private Border? _error;

    /// <summary>
    /// Calculates and updates the statistics for office and home office days.
    /// This method adjusts the grid layout proportions, updates the text elements
    /// with percentage values, configures tooltips with day counts, and manages the
    /// visibility of warning and error indicators based on pre-defined thresholds.
    /// </summary>
    private void CalculateStats()
    {
        if (_grid is null) return;
        if (_homeOfficePercentageText is null) return;
        if (_officePercentageText is null) return;
        if (_homeStackPanel is null) return;
        if (_officeStackPanel is null) return;
        if (_warning is null) return;
        if (_error is null) return;

        _grid.ColumnDefinitions[0].Width = new GridLength(HomeOfficePercentage, GridUnitType.Star);
        _grid.ColumnDefinitions[1].Width = new GridLength(OfficePercentage, GridUnitType.Star);

        _homeOfficePercentageText.Text = $"{HomeOfficePercentage:F2}%";
        _officePercentageText.Text = $"{OfficePercentage:F2}%";

        _homeStackPanel.SetValue(ToolTip.TipProperty, $"{HomeOfficeDays} Tage");
        _officeStackPanel.SetValue(ToolTip.TipProperty, $"{OfficeDays} Tage");

        _error.IsVisible = HomeOfficePercentage > 50.00;
        _warning.IsVisible = HomeOfficePercentage == 50.00;
    }

    #endregion
}
