namespace OfficeTracker.Database.Factories;

public sealed class OtDesignTimeFactory : IDesignTimeDbContextFactory<OtContext>
{
	public OtContext CreateDbContext(string[] args)
	{
		var optionsBuilder = new DbContextOptionsBuilder<OtContext>();
		optionsBuilder.UseSqlite($"Data Source={Options.DB_NAME}");
		return new OtContext(optionsBuilder.Options);
	}
}
