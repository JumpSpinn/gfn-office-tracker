namespace OfficeTracker.Core.Entities;

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
	public uint HomeOfficeTargetQuoted { get; set; }
	public uint OfficeTargetQuoted { get; set; }

	public DateTime WeekStartDate => WeekDays[0].Date;
	public DateTime WeekEndDate => WeekDays[^1].Date;
}
