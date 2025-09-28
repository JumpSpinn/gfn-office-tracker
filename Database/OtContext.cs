namespace OfficeTracker.Database;

public sealed partial class OtContext(DbContextOptions<OtContext> options) : DbContext(options)
{
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		=> base.OnConfiguring(optionsBuilder);

	protected override void OnModelCreating(ModelBuilder modelBuilder)
		=> base.OnModelCreating(modelBuilder);
}
