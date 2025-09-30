namespace OfficeTracker.Components;

/// <summary>
/// Represents a custom UI component that displays indicators for days of the week.
/// </summary>
public class WeekIndicatorComponent : TemplatedControl
{
	/// <summary>
	/// Identifies a styled property representing the short label for a day of the week.
	/// </summary>
	public static readonly StyledProperty<string> ShortWeekDayLabelProperty =
		AvaloniaProperty.Register<PlannableDayListControl, string>(nameof(ShortWeekDayLabel), defaultValue: "-");

	public string ShortWeekDayLabel
	{
		get => GetValue(ShortWeekDayLabelProperty);
		set => SetValue(ShortWeekDayLabelProperty, value);
	}

	/// <summary>
	/// Identifies a styled property representing the color configuration for a specific day type.
	/// </summary>
	public static readonly StyledProperty<PlannableDayColorPair> DayTypeColorProperty =
		AvaloniaProperty.Register<PlannableDayListControl, PlannableDayColorPair>(nameof(DayType), defaultValue: new PlannableDayColorPair("#FFFFFF", "#FFFFFF"));

	public PlannableDayColorPair DayTypeColor
	{
		get => GetValue(DayTypeColorProperty);
		set => SetValue(DayTypeColorProperty, value);
	}
}
