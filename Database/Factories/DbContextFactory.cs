namespace OfficeTracker.Database.Factories;

using Microsoft.EntityFrameworkCore;

public sealed class DbContextFactory(DbContextOptions<DbContext> options) : IDbContextFactory<DbContext>
{
	public DbContext CreateDbContext() => new(options);
}
