namespace OfficeTracker.Core.Entities;

/// <summary>
/// Represents a day of the week with associated details such as type, date, and color.
/// </summary>
public sealed class WeekDayEntity
{
	public DayType Type { get; set; } = DayType.NONE;
	public DateTime Date { get; set; } = DateTime.MinValue;
	public string HexColor { get; set; } = string.Empty;
	public string TypeName =>
		Type switch
		{
			DayType.HOME => "HomeOffice",
			DayType.OFFICE => "Standort",
			_ => "Unknown"
		};
}
