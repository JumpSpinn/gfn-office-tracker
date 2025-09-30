namespace OfficeTracker.Models;

/// <summary>
/// Represents a day of the week with associated details such as type, date, and color.
/// </summary>
public sealed class WeekDay
{
	public DayType Type { get; set; } = DayType.NONE;
	public DateTime Date { get; set; } = DateTime.MinValue;
	public string HexColor { get; set; } = string.Empty;
}
