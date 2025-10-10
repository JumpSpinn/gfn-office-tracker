namespace OfficeTracker.Infrastructure.Database.Factories;

/// <summary>
/// Provides a factory for creating instances of the OtContext database context.
/// This class is responsible for configuring and initializing the database context.
/// The context is configured to use the SQLite database engine and the database file
/// specified in the application's configuration.
/// </summary>
public class OtContextFactory : IDbContextFactory<OtContext>
{
	private readonly ConfigController _configController;
	private readonly LogController _logController;

	public OtContextFactory(ConfigController cc, LogController lc)
	{
		_configController = cc;
		_logController = lc;
	}

	public OtContext CreateDbContext()
	{
		var optionsBuilder = new DbContextOptionsBuilder<OtContext>();
		// ReSharper disable once RedundantAssignment
		var dbPath = Path.Combine(_configController.ConfigEntity.DatabasePath, Options.DB_NAME);
#if DEBUG
		dbPath = Options.DB_NAME;
#endif

		_logController.Debug($"Database path: {dbPath}");
		optionsBuilder.UseSqlite($"Data Source={dbPath}");

#if DEBUG
		optionsBuilder
			.EnableSensitiveDataLogging()
			.EnableDetailedErrors()
			.LogTo(Console.WriteLine, LogLevel.Information);
#else
		optionsBuilder.EnableDetailedErrors();
#endif

		return new OtContext(optionsBuilder.Options);
	}
}
