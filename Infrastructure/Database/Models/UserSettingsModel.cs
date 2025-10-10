namespace OfficeTracker.Infrastructure.Database.Models;

/// <summary>
/// Represents the user's settings data model for the database.
/// </summary>
[Table("user_settings")]
public sealed class UserSettingsModel
{
	[Key]
	public uint Id { get; set; }

	[MaxLength(32)]
	public string UserName { get; set; } = string.Empty;

	public uint HomeOfficeTargetQuoted { get; set; }

	public uint HomeOfficeDayCount { get; set; }

	public DayOfWeek[] HomeOfficeDays { get; set; } = [];

	public uint OfficeTargetQuoted { get; set; }

	public uint OfficeDayCount { get; set; }

	public DayOfWeek[] OfficeDays { get; set; } = [];

	public DateTime LastUpdate { get; set; } = DateTime.MinValue;
}
