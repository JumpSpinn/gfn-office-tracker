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

	#region GET

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

	#endregion

	#region CREATE

	public async Task<DbPlannableDay?> CreatePlannableDayAsync(DayType type, DateTime date)
	{
		try
		{
			await using var db = await _dbContextFactory.CreateDbContextAsync();
			var day = new DbPlannableDay()
			{
				Type = type,
				Date = date
			};
			await db.PlannableDays.AddAsync(day);
			await db.SaveChangesAsync();
			return day;
		}
		catch (Exception e)
		{
			_logController.Error(e.Message);
		}

		return null;
	}

	#endregion

	#region ADD

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

	#endregion

	#region DELETE

	public async Task<bool> DeletePlannableDayAsync(uint id)
	{
		try
		{
			await using var db = await _dbContextFactory.CreateDbContextAsync();
			var day = await db.PlannableDays.FirstOrDefaultAsync(x => x.Id == id);
			if (day is null) return false;

			db.PlannableDays.Remove(day);
			await db.SaveChangesAsync();
			return true;
		}
		catch (Exception e)
		{
			_logController.Error(e.Message);
		}

		return false;
	}

	#endregion
}
