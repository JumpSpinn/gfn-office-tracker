namespace OfficeTracker.Services;

[RegisterSingleton]
public sealed class DatabaseService(IDbContextFactory<OtContext> dbContextFactory, LogService logService)
{
	#region USER SETTINGS

	#region GET

	/// <summary>
	/// Asynchronously retrieves the user's settings from the database.
	/// </summary>
	public async Task<DbUserSettings?> GetUserSettingAsync()
	{
		try
		{
			await using var db = await dbContextFactory.CreateDbContextAsync();
			return await db.UserSettings.FirstOrDefaultAsync();
		}
		catch (Exception e)
		{
			logService.Exception(e);
		}

		return null;
	}

	/// <summary>
	/// Asynchronously retrieves the counts of home office and office days from the user's settings in the database.
	/// </summary>
	public async Task<(uint homeOfficeCount, uint officeCount, DateTime lastUpdate)?> GetDayCountsFromUserSettingsAsync()
	{
		try
		{
			await using var db = await dbContextFactory.CreateDbContextAsync();
			var data = await db.UserSettings.FirstOrDefaultAsync();
			if (data is null) return null;
			return (data.HomeOfficeDayCount,data.OfficeDayCount, data.LastUpdate);
		}
		catch(Exception e)
		{
			logService.Exception(e);
		}

		return null;
	}

	#endregion

	#region CREATE

	/// <summary>
	/// Asynchronously creates a new user setting entry in the database with the provided parameters.
	/// </summary>
	/// <param name="userName">The name of the user for whom the settings are being created.</param>
	/// <param name="homeOfficeDays">An array of days designated as home office days.</param>
	/// <param name="officeDays">An array of days designated as office days.</param>
	/// <param name="homeOfficeDayCount">The initial count of home office days for the user.</param>
	/// <param name="officeDayCount">The initial count of office days for the user.</param>
	/// <param name="homeOfficeTargetQuoted">The target number of home office days.</param>
	/// <param name="officeTargetQuoted">The target number of office days.</param>
	/// <param name="isCurrentDayTracked">Indicates whether the current day should be set as tracked.</param>
	public async Task<DbUserSettings?> CreateUserSettingAsync(string userName, DayOfWeek[] homeOfficeDays, DayOfWeek[] officeDays, uint homeOfficeDayCount, uint officeDayCount, uint homeOfficeTargetQuoted, uint officeTargetQuoted, bool isCurrentDayTracked)
	{
		try
		{
			await using var db = await dbContextFactory.CreateDbContextAsync();
			var us = new DbUserSettings()
			{
				UserName = userName,
				HomeOfficeDays = homeOfficeDays,
				OfficeDays = officeDays,
				HomeOfficeDayCount = homeOfficeDayCount,
				OfficeDayCount = officeDayCount,
				HomeOfficeTargetQuoted = homeOfficeTargetQuoted,
				OfficeTargetQuoted = officeTargetQuoted,
				LastUpdate = isCurrentDayTracked ? DateTime.Today : DateTime.MinValue
			};

			db.UserSettings.Add(us);
			await db.SaveChangesAsync();
			return await db.UserSettings.FirstOrDefaultAsync();
		}
		catch (Exception e)
		{
			logService.Exception(e);
		}

		return null;
	}

	#endregion

	#region ADD

	/// <summary>
	/// Asynchronously increments the home office day count for the user in the database
	/// and updates the last modification date.
	/// </summary>
	public async Task<uint> IncreaseHomeOfficeCountAsync()
	{
		try
		{
			await using var db = await dbContextFactory.CreateDbContextAsync();
			var data = await db.UserSettings.FirstOrDefaultAsync();
			if (data is null) return 0;

			data.HomeOfficeDayCount++;
			data.LastUpdate = DateTime.Today;
			await db.SaveChangesAsync();
			return data.HomeOfficeDayCount;
		}
		catch (Exception e)
		{
			logService.Exception(e);
		}

		return 0;
	}

	/// <summary>
	/// Asynchronously increments the office day count for the user in the database
	/// and updates the last update date.
	/// </summary>
	public async Task<uint> IncreaseOfficeCountAsync()
	{
		try
		{
			await using var db = await dbContextFactory.CreateDbContextAsync();
			var data = await db.UserSettings.FirstOrDefaultAsync();
			if (data is null) return 0;

			data.OfficeDayCount++;
			data.LastUpdate = DateTime.Today;
			await db.SaveChangesAsync();
			return data.OfficeDayCount;
		}
		catch (Exception e)
		{
			logService.Exception(e);
		}

		return 0;
	}

	#endregion

	#endregion

	#region PLANNABLE DAYS

	#region GET

	/// <summary>
	/// Asynchronously retrieves a list of plannable days from the database.
	/// The days are ordered by their date and represent plan configurations
	/// such as day type and associated metadata.
	/// </summary>
	public async Task<List<DbPlannableDay>?> GetAllPlannableDaysAsync()
	{
		try
		{
			await using var db = await dbContextFactory.CreateDbContextAsync();
			return db.PlannableDays.OrderBy(x => x.Date).ToList();
		}
		catch (Exception e)
		{
			logService.Exception(e);
		}

		return null;
	}

	/// <summary>
	/// Asynchronously retrieves a specific plannable day from the database based on the provided date.
	/// If no entry matches the given date, null is returned.
	/// </summary>
	public async Task<DbPlannableDay?> GetSinglePlannableDayAsync(DateTime dt)
	{
		try
		{
			await using var db = await dbContextFactory.CreateDbContextAsync();
			var day = await db.PlannableDays.FirstOrDefaultAsync(x => x.Date == dt);
			return day;
		}
		catch (Exception e)
		{
			logService.Exception(e);
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
			await using var db = await dbContextFactory.CreateDbContextAsync();
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
			logService.Exception(e);
		}

		return null;
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
			await using var db = await dbContextFactory.CreateDbContextAsync();
			var day = await db.PlannableDays.FirstOrDefaultAsync(x => x.Id == id);
			if (day is null) return false;

			db.PlannableDays.Remove(day);
			await db.SaveChangesAsync();
			return true;
		}
		catch (Exception e)
		{
			logService.Exception(e);
		}

		return false;
	}

	#endregion

	#endregion
}
