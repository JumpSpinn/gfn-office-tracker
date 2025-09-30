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
	public WeekDay Monday => WeekDays.Length > 0 ? WeekDays[0] : new WeekDay();
	public WeekDay Tuesday => WeekDays.Length > 1 ? WeekDays[1] : new WeekDay();
	public WeekDay Wednesday => WeekDays.Length > 2 ? WeekDays[2] : new WeekDay();
	public WeekDay Thursday => WeekDays.Length > 3 ? WeekDays[3] : new WeekDay();
	public WeekDay Friday => WeekDays.Length > 4 ? WeekDays[4] : new WeekDay();
	public WeekDay Saturday => WeekDays.Length > 5 ? WeekDays[5] : new WeekDay();
	public WeekDay Sunday => WeekDays.Length > 6 ? WeekDays[6] : new WeekDay();
}
