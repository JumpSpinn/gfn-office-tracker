namespace OfficeTracker.Core.Entities;

/// <summary>
/// Represents runtime configuration data for a user's work schedule and targets.
/// </summary>
public sealed class RuntimeData
{
	public string UserName { get; set; } = string.Empty;

	public uint HomeOfficeTargetQuoted { get; set; }

	public DayOfWeek[] HomeOfficeDays { get; set; } = [];

	public uint OfficeTargetQuoted { get; set; }

	public DayOfWeek[] OfficeDays { get; set; } = [];
}
