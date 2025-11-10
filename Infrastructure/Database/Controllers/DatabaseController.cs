namespace OfficeTracker.Infrastructure.Database.Controllers;

[RegisterSingleton]
public sealed class DatabaseController
{
	private readonly LogController _logController;
	private readonly IDbContextFactory<OtContext> _dbContext;

	public DatabaseController(LogController lc, IDbContextFactory<OtContext> dbContext, MainWindowEvents mwe)
	{
		_logController = lc;
		_dbContext = dbContext;
		mwe.OnStarted += DeletePlannableDayInPastAsync;
		mwe.OnStarted += DeleteHolidaysInPastAsync;
	}

	/// <summary>
	/// Initializes the database connection, applies any pending migrations, and verifies the database state.
	/// </summary>
	public async Task<(bool HasData, bool Result)> InitializeAsync()
	{
	    try
	    {
	       await using var db = await _dbContext.CreateDbContextAsync();

	       var canConnect = await db.Database.CanConnectAsync();
	       if (!canConnect)
	       {
	          _logController.Error("Could not connect to database.");
	          return (false, false);
	       }

	       _logController.Info("Connected to database.");

	       var appliedMigrations = (await db.Database.GetAppliedMigrationsAsync()).ToList();
	       var allMigrations = db.Database.GetMigrations().ToList();
	       var pendingMigrations = (await db.Database.GetPendingMigrationsAsync()).ToList();

	       _logController.Debug($"Applied migrations: {appliedMigrations.Count}");
	       _logController.Debug($"All migrations: {allMigrations.Count}");
	       _logController.Debug($"Pending migrations: {pendingMigrations.Count}");

	       foreach (var pending in pendingMigrations)
		       _logController.Debug($"Pending: {pending}");

	       if (appliedMigrations.Count == 0 && allMigrations.Count > 0)
	       {
	          _logController.Debug("Database exists but has no migration history. Fixing...");

	          await db.Database.ExecuteSqlRawAsync(
	             "CREATE TABLE IF NOT EXISTS __EFMigrationsHistory (MigrationId TEXT NOT NULL PRIMARY KEY, ProductVersion TEXT NOT NULL)");

	          foreach (var migration in allMigrations)
		          await db.Database.ExecuteSqlAsync(
			          $"INSERT OR IGNORE INTO __EFMigrationsHistory (MigrationId, ProductVersion) VALUES ({migration}, '8.0.0')");

	          _logController.Debug($"Added {allMigrations.Count} migrations to history table.");
	       }

	       _logController.Info("Starting MigrateAsync...");
	       await db.Database.MigrateAsync();
	       _logController.Info("MigrateAsync completed.");

	       var hasData = db.UserSettings.Any();

	       return (hasData, true);
	    }
	    catch (Exception e)
	    {
	       _logController.Exception(e);
	    }

	    return (false, false);
	}

	/// <summary>
	/// Create a backup of the database to the new save directory.
	/// </summary>
	public async Task<bool> BackupDatabaseAsync(string path)
	{
		try
		{
			var db = await _dbContext.CreateDbContextAsync();
			var originalConnection = db.Database.GetDbConnection() as SqliteConnection;
			if (originalConnection is null)
			{
				_logController.Error("Could not get connection to database.");
				return false;
			}

			var combinedPath = Path.Combine(path, Options.DB_NAME);
			var newConnection = new SqliteConnection($"Data Source={combinedPath}");
			await originalConnection.OpenAsync();
			originalConnection.BackupDatabase(newConnection);
			await originalConnection.CloseAsync();
			_logController.Info($"Database backup created at path: {combinedPath}");

			return true;
		}
		catch (Exception e)
		{
			_logController.Exception(e);
		}

		return false;
	}

	#region PLANNABLE DAYS

	/// <summary>
	/// Removes all plannable days from the database that have a date in the past.
	/// </summary>
	private async Task DeletePlannableDayInPastAsync()
	{
		try
		{
			await using var db = await _dbContext.CreateDbContextAsync();
			var days = await db.PlannableDays.Where(x => x.Date < DateTime.Today).ToListAsync();
			db.PlannableDays.RemoveRange(days);
			_logController.Debug($"Deleted {days.Count} plannable days in the past.");
			await db.SaveChangesAsync();
		}
		catch (Exception e)
		{
			_logController.Exception(e);
		}
	}

	#endregion

	#region HOLIDAYS

	/// <summary>
	/// Deletes all holidays from the database that have both their start and end dates in the past.
	/// </summary>
	private async Task DeleteHolidaysInPastAsync()
	{
		try
		{
			await using var db = await _dbContext.CreateDbContextAsync();
			var holidays = await db.Holidays.Where(x => x.StartDate < DateTime.Today && x.EndDate < DateTime.Today).ToListAsync();
			db.Holidays.RemoveRange(holidays);
			_logController.Debug($"Deleted {holidays.Count} holidays in the past.");
			await db.SaveChangesAsync();
		}
		catch (Exception e)
		{
			_logController.Exception(e);
		}
	}

	#endregion
}
