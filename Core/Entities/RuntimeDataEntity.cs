namespace OfficeTracker.Core.Entities;

/// <summary>
/// Represents runtime configuration data for a user's work schedule and targets.
/// </summary>
public sealed class RuntimeDataEntity
{
	public string UserName { get; set; } = string.Empty;

	public uint HomeOfficeTargetQuoted { get; set; }

	public DayOfWeek[] HomeOfficeDays { get; set; } = [];

	public uint OfficeTargetQuoted { get; set; }

	public DayOfWeek[] OfficeDays { get; set; } = [];

	public (DayOfWeek Day, DayType Type)[] DayOfWeeks => HomeOfficeDays
		.Select(d => (Day: d, Type: DayType.HOME))
		.Concat(OfficeDays
			.Select(d => (Day: d, Type: DayType.OFFICE)))
		.OrderBy(item => item.Day)
		.ToArray();
}
