namespace OfficeTracker.Database.Factories;

public sealed class OfDesignTimeFactory : IDesignTimeDbContextFactory<OfContext>
{
	public OfContext CreateDbContext(string[] args)
	{
		var optionsBuilder = new DbContextOptionsBuilder<OfContext>();
		optionsBuilder.UseSqlite($"Data Source={Options.DB_NAME}");
		return new OfContext(optionsBuilder.Options);
	}
}
