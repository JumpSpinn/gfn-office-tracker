namespace OfficeTracker.Database.Models;

/// <summary>
/// Represents general statistical data stored in the database table "general".
/// This class is used to track and persist primary statistics such as home office days, office days,
/// and the last updated timestamp for these statistics.
/// </summary>
[Table("general")]
public sealed class DbGeneral
{
	[Key]
	public uint Id { get; set; }

	public uint HomeOfficeDays { get; set; }

	public uint OfficeDays { get; set; }

	public DateTime LastUpdate { get; set; }
}
