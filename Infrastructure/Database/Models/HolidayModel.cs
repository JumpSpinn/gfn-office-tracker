namespace OfficeTracker.Infrastructure.Database.Models;

/// <summary>
/// Represents a holiday entity in the database.
/// This class is used to manage the holidays and their respective details within the application.
/// </summary>
[Table("holidays")]
public sealed class HolidayModel
{
	[Key]
	public uint Id { get; set; }

	public string Name { get; set; } = string.Empty;

	public DateTime StartDate { get; set; }

	public DateTime EndDate { get; set; }
}
