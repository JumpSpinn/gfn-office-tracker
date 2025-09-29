namespace OfficeTracker.Services.Forms;

/// <summary>
/// Provides services related to statistical form data management, specifically for creating
/// and managing general statistical data in the application. It handles interactions
/// with the database through <see cref="OtContext"/> and records logs for operations
/// using the <see cref="LogController"/>.
/// </summary>
[RegisterSingleton]
public sealed class StatsFormService
{
	private readonly IDbContextFactory<OtContext> _dbContextFactory;
	private readonly LogController _logController;

	public StatsFormService(IDbContextFactory<OtContext> dbContextFactory, LogController logController)
	{
		_dbContextFactory = dbContextFactory;
		_logController = logController;
	}

	#region CREATE

	/// <summary>
	/// Creates a new general statistical data entry in the database with the specified parameters.
	/// </summary>
	public async Task<DbGeneral?> CreateGeneralDataAsync(uint homeOfficeDays, uint officeDays, bool hasBeenDayCounted)
	{
		try
		{
			await using var db = await _dbContextFactory.CreateDbContextAsync();
			DbGeneral general = new()
			{
				HomeOfficeDays = homeOfficeDays,
				OfficeDays = officeDays,
				LastUpdate = hasBeenDayCounted ? DateTime.Today : DateTime.MinValue
			};
			db.General.Add(general);
			await db.SaveChangesAsync();
			return general;
		}
		catch (Exception e)
		{
			_logController.Error(e.Message);
		}
		return null;
	}

	#endregion
}
