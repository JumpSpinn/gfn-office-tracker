namespace OfficeTracker.Infrastructure.Database.Services;

[RegisterSingleton]
public sealed class DatabaseService(IDbContextFactory<OtContext> dbContextFactory, LogController logController)
{
	#region USER SETTINGS

	#region GET

	/// <summary>
	/// Asynchronously retrieves the user's settings from the database.
	/// </summary>
	public async Task<UserSettingsModel?> GetUserSettingAsync()
	{
		try
		{
			await using var db = await dbContextFactory.CreateDbContextAsync();
			return await db.UserSettings.FirstOrDefaultAsync();
		}
		catch (Exception e)
		{
			logController.Exception(e);
		}

		return null;
	}

	/// <summary>
	/// Asynchronously retrieves the user's name from the database.
	/// </summary>
	public async Task<string?> GetUserNameAsync()
	{
		try
		{
			await using var db = await dbContextFactory.CreateDbContextAsync();
			var data = await db.UserSettings.FirstOrDefaultAsync();
			return data?.UserName;
		}
		catch (Exception e)
		{
			logController.Exception(e);
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
			logController.Exception(e);
		}

		return null;
	}

	#endregion

	#region CREATE

	/// <summary>
	/// Asynchronously creates a new user setting entry in the database with the provided parameters.
	/// </summary>
	public async Task<UserSettingsModel?> CreateUserSettingAsync(string userName, DayOfWeek[] homeOfficeDays, DayOfWeek[] officeDays, uint homeOfficeDayCount, uint officeDayCount, uint homeOfficeTargetQuoted, uint officeTargetQuoted, bool isCurrentDayTracked)
	{
		try
		{
			await using var db = await dbContextFactory.CreateDbContextAsync();
			var us = new UserSettingsModel()
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
			logController.Exception(e);
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
			logController.Exception(e);
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
			logController.Exception(e);
		}

		return 0;
	}

	#endregion

	#region UPDATE

	/// <summary>
	/// Asynchronously updates the user's name in the database.
	/// </summary>
	public async Task<string?> UpdateUserNameAsync(string userName)
	{
		try
		{
			await using var db = await dbContextFactory.CreateDbContextAsync();
			var data = await db.UserSettings.FirstOrDefaultAsync();
			if (data is null) return null;
			data.UserName = userName;
			await db.SaveChangesAsync();
			return data.UserName;
		}
		catch (Exception e)
		{
			logController.Exception(e);
		}

		return null;
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
	public async Task<List<PlannableDayModel>?> GetAllPlannableDaysAsync()
	{
		try
		{
			await using var db = await dbContextFactory.CreateDbContextAsync();
			return db.PlannableDays.OrderBy(x => x.Date).ToList();
		}
		catch (Exception e)
		{
			logController.Exception(e);
		}

		return null;
	}

	/// <summary>
	/// Asynchronously retrieves a specific plannable day from the database based on the provided date.
	/// If no entry matches the given date, null is returned.
	/// </summary>
	public async Task<PlannableDayModel?> GetSinglePlannableDayByDateAsync(DateTime dt)
	{
		try
		{
			await using var db = await dbContextFactory.CreateDbContextAsync();
			var day = await db.PlannableDays.FirstOrDefaultAsync(x => x.Date == dt);
			return day;
		}
		catch (Exception e)
		{
			logController.Exception(e);
		}

		return null;
	}

	#endregion

	#region CREATE

	/// <summary>
	/// Asynchronously creates a new plannable day entry in the database.
	/// The created entry represents an office-related or home-related day, depending on the specified type and date.
	/// </summary>
	public async Task<PlannableDayModel?> CreatePlannableDayAsync(DayType type, DateTime date)
	{
		try
		{
			await using var db = await dbContextFactory.CreateDbContextAsync();
			var day = new PlannableDayModel()
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
			logController.Exception(e);
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
			logController.Exception(e);
		}

		return false;
	}

	#endregion

	#endregion

	#region HOLIDAYS

	#region GET

	/// <summary>
	/// Asynchronously retrieves all holidays from the database in an ordered list by start date.
	/// </summary>
	public async Task<List<HolidayModel>?> GetAllHolidaysAsync()
	{
		try
		{
			await using var db = await dbContextFactory.CreateDbContextAsync();
			return db.Holidays.OrderBy(x => x.StartDate).ToList();
		}
		catch (Exception e)
		{
			logController.Exception(e);
		}

		return null;
	}

	/// <summary>
	/// Asynchronously retrieves a single holiday from the database that matches the specified start and end dates.
	/// </summary>
	public async Task<HolidayModel?> GetSingleHolidayByStartEndDateAsync(DateTime start, DateTime end)
	{
		try
		{
			await using var db = await dbContextFactory.CreateDbContextAsync();
			var holiday = await db.Holidays.FirstOrDefaultAsync(x => x.StartDate == start && x.EndDate == end);
			return holiday;
		}
		catch (Exception e)
		{
			logController.Exception(e);
		}

		return null;
	}

	/// <summary>
	/// Asynchronously retrieves a single holiday from the database that overlaps with the specified date.
	/// </summary>
	public async Task<HolidayModel?> GetSingleHolidayByDateAsync(DateTime dt)
	{
		try
		{
			await using var db = await dbContextFactory.CreateDbContextAsync();
			var holiday = await db.Holidays.FirstOrDefaultAsync(x => x.StartDate <= dt && x.EndDate >= dt);
			return holiday;
		}
		catch (Exception e)
		{
			logController.Exception(e);
		}

		return null;
	}

	#endregion

	#region CREATE

	/// <summary>
	/// Asynchronously creates a new holiday and saves it to the database.
	/// </summary>
	public async Task<HolidayModel?> CreateHolidayAsync(string name, DateTime start, DateTime end)
	{
		try
		{
			await using var db = await dbContextFactory.CreateDbContextAsync();
			var holiday = new HolidayModel()
			{
				Name = name,
				StartDate = start,
				EndDate = end
			};
			await db.Holidays.AddAsync(holiday);
			await db.SaveChangesAsync();
			return holiday;
		}
		catch (Exception e)
		{
			logController.Exception(e);
		}

		return null;
	}

	#endregion

	#region DELETE

	/// <summary>
	/// Asynchronously deletes a holiday record from the database based on the provided holiday ID.
	/// </summary>
	public async Task<bool> DeleteHolidayAsync(uint id)
	{
		try
		{
			await using var db = await dbContextFactory.CreateDbContextAsync();
			var day = await db.Holidays.FirstOrDefaultAsync(x => x.Id == id);
			if (day is null) return false;

			db.Holidays.Remove(day);
			await db.SaveChangesAsync();
			return true;
		}
		catch (Exception e)
		{
			logController.Exception(e);
		}

		return false;
	}

	#endregion

	#endregion
}
