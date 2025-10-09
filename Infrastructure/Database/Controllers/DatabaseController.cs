namespace OfficeTracker.Infrastructure.Database.Controllers;

[RegisterSingleton]
public sealed class DatabaseController
{
	private readonly LogController _logController;
	private readonly IDbContextFactory<OtContext> _dbContext;

	public DatabaseController(LogController lc, IDbContextFactory<OtContext> dbContext)
	{
		_logController = lc;
		_dbContext = dbContext;
	}

	/// <summary>
	/// Checks if the database is created and checks if it contains any data.
	/// </summary>
	public async Task<(bool HasData, bool Result)> InitializeAsync()
	{
		try
		{
			await using var db = await _dbContext.CreateDbContextAsync();
			await db.Database.EnsureCreatedAsync(); // create/migrate database
			var hasData = db.UserSettings.Any();

			return (hasData, true);
		}
		catch (Exception e)
		{
			_logController.Exception(e);
		}

		return (false, false);
	}
}
