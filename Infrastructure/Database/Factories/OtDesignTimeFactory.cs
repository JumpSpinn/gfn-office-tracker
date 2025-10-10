namespace OfficeTracker.Infrastructure.Database.Factories;

/// <summary>
/// A factory class to create instances of the <see cref="OtContext"/> class
/// at design time. This is commonly used by tools such as EF Core migrations
/// to initialize the database context without requiring runtime dependency
/// injection setup.
/// </summary>
public sealed class OtDesignTimeFactory : IDesignTimeDbContextFactory<OtContext>
{
	/// <summary>
	/// Creates an instance of the <see cref="OtContext"/> class with the configured
	/// connection options. This method is primarily intended to be used at design time
	/// to set up the database context for tasks such as EF Core migrations.
	/// </summary>
	public OtContext CreateDbContext(string[] args)
	{
		var optionsBuilder = new DbContextOptionsBuilder<OtContext>();
		optionsBuilder.UseSqlite($"Data Source={Options.DB_NAME}");
		return new OtContext(optionsBuilder.Options);
	}
}
