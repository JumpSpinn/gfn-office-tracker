namespace OfficeTracker.Database;

public sealed partial class OfContext(DbContextOptions<DbContext> options) : DbContext(options)
{
	protected override void OnModelCreating(ModelBuilder modelBuilder)
		=> base.OnModelCreating(modelBuilder);
}
