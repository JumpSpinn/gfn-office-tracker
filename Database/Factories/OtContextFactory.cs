namespace OfficeTracker.Database.Factories;

/// <summary>
/// Provides a factory for creating instances of the OtContext database context.
/// Implements the IDbContextFactory interface to enable on-demand creation of OtContext
/// instances, which are configured with the provided DbContextOptions.
/// </summary>
public sealed class OtContextFactory(DbContextOptions<OtContext> options) : IDbContextFactory<OtContext>
{
	public OtContext CreateDbContext() => new(options);
}
