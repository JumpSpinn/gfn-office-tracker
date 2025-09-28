namespace OfficeTracker.Database.Factories;

public sealed class OtContextFactory(DbContextOptions<OtContext> options) : IDbContextFactory<OtContext>
{
	public OtContext CreateDbContext() => new(options);
}
