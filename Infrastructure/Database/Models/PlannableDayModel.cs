namespace OfficeTracker.Infrastructure.Database.Models;

/// <summary>
/// Represents a plannable day entity stored in the database.
/// This class maps to the "plannable_days" table and contains properties related to the type, date, and status of a day.
/// It also provides logic for determining associated colors based on the day type.
/// </summary>
[Table("plannable_days")]
public sealed class PlannableDayModel
{
	[Key]
	public uint Id { get; set; }

	public DayType Type { get; set; }

	public DateTime Date { get; set; }

	public bool IsDeleted { get; set; }

	/// <summary>
	/// Gets a <see cref="PlannableDayColorPairEntity"/> that represents the colors associated with
	/// the current day's type. The colors are determined based on the <see cref="DayType"/>
	/// of the instance.
	/// </summary>
	public PlannableDayColorPairEntity TypeColors => Type switch
	{
		DayType.HOME => new PlannableDayColorPairEntity("#30003764", "#003764"),
		DayType.OFFICE => new PlannableDayColorPairEntity("#30357a32", "#357a32"),
		_ => new PlannableDayColorPairEntity("#40640000", "#640000")
	};
}
