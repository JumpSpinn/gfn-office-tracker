namespace OfficeTracker.Models;

/// <summary>
/// Represents a calculated week, including information about the days in the week,
/// the name of the week, and the distribution of home office and office days.
/// </summary>
public sealed class CalculatedWeek
{
	public WeekDay[] WeekDays { get; set; } = [];
	public string WeekName { get; set; } = string.Empty;
	public uint HomeOfficeDays { get; set; }
	public uint OfficeDays { get; set; }
}
