namespace OfficeTracker.Components;

/// <summary>
/// Represents a custom UI component that displays indicators for days of the week.
/// </summary>
public sealed class WeekIndicatorComponent : TemplatedControl
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
	/// Identifies a styled property representing the hex color associated with a specific type of day.
	/// </summary>
	public static readonly StyledProperty<string> DayTypeHexColorProperty =
		AvaloniaProperty.Register<PlannableDayListControl, string>(nameof(DayTypeHexColor), defaultValue: "#FFFFFF");

	public string DayTypeHexColor
	{
		get => GetValue(DayTypeHexColorProperty);
		set => SetValue(DayTypeHexColorProperty, value);
	}
}
