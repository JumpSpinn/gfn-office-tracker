namespace OfficeTracker.Models;

/// <summary>
/// Represents a calculated week, including information about the days in the week,
/// the name of the week, and the distribution of home office and office days.
/// </summary>
public sealed class CalculatedWeekModel
{
	public WeekDayModel[] WeekDays { get; set; } = [];
	public string WeekName { get; set; } = string.Empty;
	public uint HomeOfficeDays { get; set; }
	public uint OfficeDays { get; set; }
	public WeekDayModel Monday => WeekDays.Length > 0 ? WeekDays[0] : new WeekDayModel();
	public WeekDayModel Tuesday => WeekDays.Length > 1 ? WeekDays[1] : new WeekDayModel();
	public WeekDayModel Wednesday => WeekDays.Length > 2 ? WeekDays[2] : new WeekDayModel();
	public WeekDayModel Thursday => WeekDays.Length > 3 ? WeekDays[3] : new WeekDayModel();
	public WeekDayModel Friday => WeekDays.Length > 4 ? WeekDays[4] : new WeekDayModel();
	public WeekDayModel Saturday => WeekDays.Length > 5 ? WeekDays[5] : new WeekDayModel();
	public WeekDayModel Sunday => WeekDays.Length > 6 ? WeekDays[6] : new WeekDayModel();
}
