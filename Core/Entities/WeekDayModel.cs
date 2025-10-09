namespace OfficeTracker.Core.Entities;

using Enums;

/// <summary>
/// Represents a day of the week with associated details such as type, date, and color.
/// </summary>
public sealed class WeekDayModel
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
