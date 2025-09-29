namespace OfficeTracker.Database.Models;

[Table("plannable_days")]
public sealed class DbPlannableDay
{
	[Key]
	public uint Id { get; set; }

	public DayType Type { get; set; }

	public DateTime Date { get; set; }

	public bool IsDeleted { get; set; }

	public PlannableDayColorPair TypeColors => Type switch
	{
		DayType.HOME => new PlannableDayColorPair("#30003764", "#003764"),
		DayType.OFFICE => new PlannableDayColorPair("#30357a32", "#357a32"),
		_ => new PlannableDayColorPair("#40640000", "#640000")
	};
}
