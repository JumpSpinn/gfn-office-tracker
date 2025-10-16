namespace OfficeTracker.Infrastructure.Database;

/// <summary>
/// Represents the database context for the OfficeTracker application,
/// configured to manage and interact with database models and operations.
/// </summary>
public sealed partial class OtContext(DbContextOptions<Infrastructure.Database.OtContext> options) : DbContext(options)
{
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		=> base.OnConfiguring(optionsBuilder);

	protected override void OnModelCreating(ModelBuilder modelBuilder)
		=> base.OnModelCreating(modelBuilder);
}
