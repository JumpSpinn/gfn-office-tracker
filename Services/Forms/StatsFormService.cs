namespace OfficeTracker.Services.Forms;

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
