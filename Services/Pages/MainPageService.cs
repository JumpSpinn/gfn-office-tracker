namespace OfficeTracker.Services.Pages;

[RegisterSingleton]
public sealed class MainPageService
{
	private readonly IDbContextFactory<OtContext> _dbContextFactory;
	private readonly LogController _logController;

	public MainPageService(IDbContextFactory<OtContext> dbContextFactory, LogController logController)
	{
		_dbContextFactory = dbContextFactory;
		_logController = logController;
	}

	public async Task<DbGeneral?> GetGeneralDataAsync()
	{
		try
		{
			await using var db = await _dbContextFactory.CreateDbContextAsync();
			return await db.General.FirstOrDefaultAsync();
		}
		catch (Exception e)
		{
			_logController.Error(e.Message);
		}

		return null;
	}

	public async Task<uint> AddHomeOfficeDayAsync()
	{
		try
		{
			await using var db = await _dbContextFactory.CreateDbContextAsync();
			var data = await db.General.FirstOrDefaultAsync();
			if (data is null) return 0;

			data.HomeOfficeDays++;
			await db.SaveChangesAsync();
			return data.HomeOfficeDays;
		}
		catch (Exception e)
		{
			_logController.Error(e.Message);
		}

		return 0;
	}

	public async Task<uint> AddOfficeDayAsync()
	{
		try
		{
			await using var db = await _dbContextFactory.CreateDbContextAsync();
			var data = await db.General.FirstOrDefaultAsync();
			if (data is null) return 0;

			data.OfficeDays++;
			await db.SaveChangesAsync();
			return data.OfficeDays;
		}
		catch (Exception e)
		{
			_logController.Error(e.Message);
		}

		return 0;
	}
}
