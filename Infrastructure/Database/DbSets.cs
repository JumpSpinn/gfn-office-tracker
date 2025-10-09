namespace OfficeTracker.Infrastructure.Database;

/// <summary>
/// Represents the Entity Framework Core database context for the OfficeTracker application.
/// The context provides access to the relevant DbSets and is responsible for configuring and interacting with the database.
/// </summary>
public sealed partial class OtContext
{
	public DbSet<DbUserSettings> UserSettings { get; set; }
	public DbSet<DbPlannableDay> PlannableDays { get; set; }
}
