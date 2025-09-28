namespace OfficeTracker.Database;

public sealed partial class OtContext
{
	public DbSet<DbGeneral> General { get; set; }
	public DbSet<DbPlannableDay> PlannableDays { get; set; }
}
