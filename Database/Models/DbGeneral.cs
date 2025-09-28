namespace OfficeTracker.Database.Models;

[Table("general")]
public sealed class DbGeneral
{
	[Key]
	public uint Id { get; set; }

	public uint HomeOfficeDays { get; set; }

	public uint OfficeDays { get; set; }

	public bool HasBeenDayCounted { get; set; }
}
