namespace OfficeTracker.Views.Controls;

/// <summary>
/// Represents a custom control used to display and manage office and home office statistics within an application.
/// </summary>
public class StatisticControl : TemplatedControl
{
	protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
	{
		_addButton = e.NameScope.Find<Button>("AddButton");
		if (_addButton is not null)
			_addButton.Click += OnAddButtonClick;

		_errorBorderIcon = e.NameScope.Find<Border>("ErrorIcon");
		if(_errorBorderIcon is not null)
			_errorBorderIcon.IsVisible = false;

		_warningBorderIcon = e.NameScope.Find<Border>("WarningIcon");
		if(_warningBorderIcon is not null)
			_warningBorderIcon.IsVisible = false;

		_statisticGrid = e.NameScope.Find<Grid>("StatisticGrid");

		_homeStackPanel = e.NameScope.Find<StackPanel>("HomeStackPanel");
		_homeOfficePercentage = e.NameScope.Find<TextBlock>("HomeOfficePercentage");

		_officeStackPanel = e.NameScope.Find<StackPanel>("OfficeStackPanel");
		_officePercentage = e.NameScope.Find<TextBlock>("OfficePercentage");

		base.OnApplyTemplate(e);
	}

	/// <summary>
	/// Represents a control for managing and displaying office and home office statistics. Provides functionality to calculate and visualize
	/// the ratio of home office days to office days and includes error notification through visual cues.
	/// </summary>
	static StatisticControl()
	{
		HomeOfficeDaysProperty.Changed.AddClassHandler<StatisticControl>((control, _) =>
		{
			control.ScheduleCalculationStatistic();
		});

		OfficeDaysProperty.Changed.AddClassHandler<StatisticControl>((control, _) =>
		{
			control.ScheduleCalculationStatistic();
		});
	}

	#region STATS

	public static readonly StyledProperty<uint> HomeOfficeDaysProperty =
		AvaloniaProperty.Register<StatisticControl, uint>(nameof(HomeOfficeDays), defaultValue: 0);

	public uint HomeOfficeDays
	{
		get => GetValue(HomeOfficeDaysProperty);
		set => SetValue(HomeOfficeDaysProperty, value);
	}

	public static readonly StyledProperty<uint> OfficeDaysProperty =
		AvaloniaProperty.Register<StatisticControl, uint>(nameof(OfficeDays), defaultValue: 0);

	public uint OfficeDays
	{
		get => GetValue(OfficeDaysProperty);
		set => SetValue(OfficeDaysProperty, value);
	}

	#endregion

	#region CALCULATION

	private Grid? _statisticGrid;

	private StackPanel? _homeStackPanel;
	private TextBlock? _homeOfficePercentage;

	private StackPanel? _officeStackPanel;
	private TextBlock? _officePercentage;

	private double HomeOfficePercentage
		=> (double)HomeOfficeDays / (HomeOfficeDays + OfficeDays) * 100;

	private double OfficePercentage
		=> (double)OfficeDays / (HomeOfficeDays + OfficeDays) * 100;

	/// <summary>
	/// Holds a token source used to manage cancellation of ongoing statistic calculation operations.
	/// This allows interrupting calculations when a new operation is scheduled.
	/// </summary>
	private CancellationTokenSource? _calculationCts;

	/// <summary>
	/// Schedules the calculation of statistical data relating to office and home office days.
	/// The method cancels any ongoing calculations, creates a new cancellation token, and posts
	/// a task to the user interface thread to perform the calculation asynchronously.
	/// </summary>
	private void ScheduleCalculationStatistic()
	{
		_calculationCts?.Cancel();
		_calculationCts = new CancellationTokenSource();

		var token = _calculationCts.Token;

		Dispatcher.UIThread.Post(() =>
		{
			if(!token.IsCancellationRequested)
				CalculateStatistic();
		}, DispatcherPriority.Background);
	}

	/// <summary>
	/// Recalculates and updates the visual representation of office and home office statistics.
	/// </summary>
	private void CalculateStatistic()
	{
		if (_statisticGrid is null) return;
		if (_homeOfficePercentage is null || _officePercentage is null) return;
		if (_homeStackPanel is null || _officeStackPanel is null) return;

		_statisticGrid.ColumnDefinitions[0].Width = new GridLength(HomeOfficePercentage, GridUnitType.Star);
		_statisticGrid.ColumnDefinitions[1].Width = new GridLength(OfficePercentage, GridUnitType.Star);

		_homeOfficePercentage.Text = $"{HomeOfficePercentage:F2}%";
		_officePercentage.Text = $"{OfficePercentage:F2}%";

		_homeStackPanel.SetValue(ToolTip.TipProperty, $"{HomeOfficeDays} Tage");
		_officeStackPanel.SetValue(ToolTip.TipProperty, $"{OfficeDays} Tage");

		if(HomeOfficePercentage > 50.00)
			ShowErrorBorderIcon();
		// ReSharper disable once CompareOfFloatsByEqualityOperator
		else if(HomeOfficePercentage == 50.00)
			ShowWarningBorderIcon();
		else
			HideAllBorderIcons();
	}

	#endregion

	#region ICONS

	private Border? _errorBorderIcon;
	private Border? _warningBorderIcon;

	/// <summary>
	/// Makes the error border icon visible and ensures that the warning border icon is hidden.
	/// This method is invoked when certain validation or statistical criteria are not met,
	/// indicating an error state that needs to be highlighted.
	/// </summary>
	private void ShowErrorBorderIcon()
	{
		if (_errorBorderIcon is null) return;
		if (_errorBorderIcon.IsVisible) return;

		_errorBorderIcon.IsVisible = true;

		if(_warningBorderIcon is not null)
			_warningBorderIcon.IsVisible = !_errorBorderIcon.IsVisible;
	}

	/// <summary>
	/// Displays the warning border icon in the statistical control when specific conditions are met.
	/// Ensures visibility of the warning icon while maintaining consistency with the error icon's visibility.
	/// </summary>
	private void ShowWarningBorderIcon()
	{
		if (_warningBorderIcon is null) return;
		if (_warningBorderIcon.IsVisible) return;

		_warningBorderIcon.IsVisible = true;

		if(_errorBorderIcon is not null)
			_errorBorderIcon.IsVisible = !_warningBorderIcon.IsVisible;
	}

	/// <summary>
	/// Hides all border icons that are used to visually indicate warnings or errors in the control.
	/// </summary>
	private void HideAllBorderIcons()
	{
		if(_warningBorderIcon is not null && _warningBorderIcon.IsVisible)
			_warningBorderIcon.IsVisible = false;

		if(_errorBorderIcon is not null && _errorBorderIcon.IsVisible)
			_errorBorderIcon.IsVisible = false;
	}

	#endregion

	#region ADD BUTTON

	private Button? _addButton;

	/// <summary>
	/// Represents a styled property that determines whether the "Add" button in the control is enabled.
	/// When this property changes, it updates the enabled state of the associated button.
	/// </summary>
	public static readonly StyledProperty<bool> AddButtonEnabledProperty =
		AvaloniaProperty.Register<StatisticControl, bool>(nameof(AddButtonEnabled), defaultValue: false);

	public bool AddButtonEnabled
	{
		get => GetValue(AddButtonEnabledProperty);
		set => SetValue(AddButtonEnabledProperty, value);
	}

	/// <summary>
	/// Represents a styled property that determines the visibility of the "Add" button in the control.
	/// When this property changes, it updates the visibility state of the associated button.
	/// </summary>
	public static readonly StyledProperty<bool> AddButtonVisibleProperty =
		AvaloniaProperty.Register<StatisticControl, bool>(nameof(AddButtonVisible), defaultValue: false);

	public bool AddButtonVisible
	{
		get => GetValue(AddButtonVisibleProperty);
		set => SetValue(AddButtonVisibleProperty, value);
	}

	/// <summary>
	/// Defines a routed event for when the "Add" button in the control is clicked. This event
	/// uses a bubbling routing strategy and can be subscribed to for custom handling of the
	/// button click action within the control context.
	/// </summary>
	private readonly RoutedEvent<RoutedEventArgs> _addButtonClickedEvent =
		RoutedEvent.Register<StatisticControl, RoutedEventArgs>(nameof(StatisticControl), RoutingStrategies.Bubble);

	/// <summary>
	/// Represents the event triggered when the "Add" button in the StatisticControl
	/// is clicked. This event uses a bubbling routing strategy, allowing subscribers
	/// to handle the button click action within the control's event propagation.
	/// </summary>
	public event EventHandler<RoutedEventArgs> AddButtonClicked
	{
		add => AddHandler(_addButtonClickedEvent, value);
		remove => RemoveHandler(_addButtonClickedEvent, value);
	}

	/// <summary>
	/// Handles the click event for the "Add" button within the StatisticControl.
	/// This method raises the associated routed event to notify subscribers of the user action.
	/// </summary>
	private void OnAddButtonClick(object? sender, RoutedEventArgs e)
	{
		if (!AddButtonEnabled) return;
		RaiseEvent(new () { RoutedEvent = _addButtonClickedEvent});
	}

	#endregion
}

