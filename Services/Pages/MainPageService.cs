namespace OfficeTracker.Services.Pages;

/// <summary>
/// Provides functionality and operations related to the main page of the OfficeTracker application.
/// </summary>
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

	/// <summary>
	/// Asynchronously retrieves general statistical data from the database.
	/// This data includes information such as home office days, office days, and the last updated timestamp.
	/// </summary>
	public async Task<DbGeneral?> GetGeneralDataAsync()
	{
		try
		{
			await using var db = await _dbContextFactory.CreateDbContextAsync();
			_logController.Debug("Getting general data from database");
			return await db.General.FirstOrDefaultAsync();
		}
		catch (Exception e)
		{
			_logController.Error(e.Message);
		}

		return null;
	}

	/// <summary>
	/// Asynchronously retrieves a list of plannable days from the database.
	/// The days are ordered by their date and represent plan configurations
	/// such as day type and associated metadata.
	/// </summary>
	public async Task<List<DbPlannableDay>?> GetPlannableDaysAsync()
	{
		try
		{
			await using var db = await _dbContextFactory.CreateDbContextAsync();
			_logController.Debug("Getting plannable days from database");
			return db.PlannableDays.OrderBy(x => x.Date).ToList();
		}
		catch (Exception e)
		{
			_logController.Error(e.Message);
		}

		return null;
	}

	#endregion

	#region CREATE

	/// <summary>
	/// Asynchronously creates a new plannable day entry in the database.
	/// The created entry represents an office-related or home-related day, depending on the specified type and date.
	/// </summary>
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
			_logController.Debug($"Created new plannable day entry #{day.Id} on date {day.Date} with type {day.Type}");
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

	/// <summary>
	/// Asynchronously increments the count of home office days in the database
	/// and updates the last modification date to the current date.
	/// </summary>
	public async Task<uint> AddHomeOfficeDayAsync()
	{
		try
		{
			await using var db = await _dbContextFactory.CreateDbContextAsync();
			var data = await db.General.FirstOrDefaultAsync();
			if (data is null) return 0;

			data.HomeOfficeDays++;
			data.LastUpdate = DateTime.Today;
			_logController.Debug($"Add home office day to database. New count: {data.HomeOfficeDays} - Last update: {data.LastUpdate}");
			await db.SaveChangesAsync();
			return data.HomeOfficeDays;
		}
		catch (Exception e)
		{
			_logController.Error(e.Message);
		}

		return 0;
	}

	/// <summary>
	/// Asynchronously increments the count of office days in the database and updates the last updated timestamp.
	/// </summary>
	public async Task<uint> AddOfficeDayAsync()
	{
		try
		{
			await using var db = await _dbContextFactory.CreateDbContextAsync();
			var data = await db.General.FirstOrDefaultAsync();
			if (data is null) return 0;

			data.OfficeDays++;
			data.LastUpdate = DateTime.Today;
			_logController.Debug($"Add office day to database. New count: {data.OfficeDays} - Last update: {data.LastUpdate}");
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

	/// <summary>
	/// Asynchronously deletes a plannable day entry from the database based on the specified identifier.
	/// </summary>
	/// <param name="id">The unique identifier of the plannable day to be deleted.</param>
	public async Task<bool> DeletePlannableDayAsync(uint id)
	{
		try
		{
			await using var db = await _dbContextFactory.CreateDbContextAsync();
			var day = await db.PlannableDays.FirstOrDefaultAsync(x => x.Id == id);
			if (day is null) return false;

			db.PlannableDays.Remove(day);
			await db.SaveChangesAsync();
			_logController.Debug($"Deleted plannable day entry #{day.Id} on date {day.Date} with type {day.Type}");
			return true;
		}
		catch (Exception e)
		{
			_logController.Error(e.Message);
		}

		return false;
	}

	#endregion

	#region CHECK

	public async Task<bool> ExistPlannableDayDateAsync(DateTime dt)
	{
		try
		{
			await using var db = await _dbContextFactory.CreateDbContextAsync();
			var day = await db.PlannableDays.FirstOrDefaultAsync(x => x.Date == dt);
			_logController.Debug($"Checking if plannable day entry for date {dt} exists.");
			return day is not null;
		}
		catch (Exception e)
		{
			_logController.Error(e.Message);
		}

		return true;
	}

	#endregion
}
