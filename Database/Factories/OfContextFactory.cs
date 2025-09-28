namespace OfficeTracker.Database.Factories;

public sealed class OfContextFactory(DbContextOptions<OfContext> options) : IDbContextFactory<OfContext>
{
	public OfContext CreateDbContext() => new(options);
}
