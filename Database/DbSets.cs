namespace OfficeTracker.Database;

public sealed partial class OfContext
{
	public DbSet<DbGeneral> General { get; set; }
	public DbSet<DbPlannableDay> PlannableDays { get; set; }
}
