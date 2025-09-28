namespace OfficeTracker.Database.Models;

[Table("plannable_days")]
public sealed class DbPlannableDay
{
	[Key]
	public uint Id { get; set; }

	public DayType Type { get; set; }

	public DateTime Date { get; set; }
}
