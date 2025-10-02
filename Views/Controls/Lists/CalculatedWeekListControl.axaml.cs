namespace OfficeTracker.Views.Controls.Lists;

public sealed class CalculatedWeekListControl : TemplatedControl
{
	/// <summary>
	/// Represents a styled property that specifies the name of the week.
	/// This property is primarily used within the `CalculatedWeekListControl` to manage or bind
	/// the display name of a week.
	/// </summary>
	public static readonly StyledProperty<string> WeekNameProperty =
		AvaloniaProperty.Register<PlannableDayListControl, string>(nameof(WeekName), defaultValue: "Woche -");

	public string WeekName
	{
		get => GetValue(WeekNameProperty);
		set => SetValue(WeekNameProperty, value);
	}

	/// <summary>
	/// Represents a styled property that specifies the starting date and time for a week.
	/// This property is primarily used within the `CalculatedWeekListControl` to manage or bind the
	/// start date of a week.
	/// </summary>
	public static readonly StyledProperty<DateTime> StartWeekDateTimeProperty =
		AvaloniaProperty.Register<PlannableDayListControl, DateTime>(nameof(StartWeekDateTime), defaultValue: DateTime.Today);

	public DateTime StartWeekDateTime
	{
		get => GetValue(StartWeekDateTimeProperty);
		set => SetValue(StartWeekDateTimeProperty, value);
	}

	/// <summary>
	/// Represents a styled property that specifies the ending date and time for a week.
	/// This property is primarily used within the `CalculatedWeekListControl` to manage or bind the
	/// end date of a week.
	/// </summary>
	public static readonly StyledProperty<DateTime> EndWeekDateTimeProperty =
		AvaloniaProperty.Register<PlannableDayListControl, DateTime>(nameof(EndWeekDateTime), defaultValue: DateTime.Today.AddDays(7));

	public DateTime EndWeekDateTime
	{
		get => GetValue(EndWeekDateTimeProperty);
		set => SetValue(EndWeekDateTimeProperty, value);
	}

	/// <summary>
	/// Represents a styled property that determines the number of home office days.
	/// This property is used in the `PlannableDayListControl` to track or bind the planned
	/// home office days within a specific week.
	/// </summary>
	public static readonly StyledProperty<uint> HomeOfficeDaysProperty =
		AvaloniaProperty.Register<PlannableDayListControl, uint>(nameof(HomeOfficeDays), defaultValue: 0);

	public uint HomeOfficeDays
	{
		get => GetValue(HomeOfficeDaysProperty);
		set => SetValue(HomeOfficeDaysProperty, value);
	}

	/// <summary>
	/// Represents a styled property that defines the number of office days.
	/// This property is used primarily within the `PlannableDayListControl` to manage
	/// or bind the count of days designated as office days.
	/// </summary>
	public static readonly StyledProperty<uint> OfficeDaysProperty =
		AvaloniaProperty.Register<PlannableDayListControl, uint>(nameof(OfficeDays), defaultValue: 0);

	public uint OfficeDays
	{
		get => GetValue(OfficeDaysProperty);
		set => SetValue(OfficeDaysProperty, value);
	}
}

