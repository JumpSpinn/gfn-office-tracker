namespace OfficeTracker.Services;

/// <summary>
/// Service responsible for calculating and managing weekly schedules based on user settings
/// and other dynamic properties. It performs operations to compute upcoming weeks
/// and tracks relevant week data.
/// </summary>
[RegisterSingleton]
public sealed class CalculateWeekService
{
	private readonly LogService _logService;
	private readonly DatabaseService _databaseService;
	private readonly MainWindowService _mainWindowService;

	public CalculateWeekService(LogService ls, DatabaseService ds, MainWindowService mws)
	{
		_logService = ls;
		_databaseService = ds;
		_mainWindowService = mws;
	}

	private uint _homeOfficeTargetQuoted;
	private uint _officeTargetQuoted;

	#region DYNAMIC PROPERTIES

	private uint _currentWeekIndex;
	private uint _currentHomeOfficeCount;
	private uint _currentOfficeCount;
	private DateTime _lastUpdateUserSettings;
	private DateTime _currentStartOfWeek;

	#endregion

	/// <summary>
	/// Resets the dynamic properties used during the calculation of weekly schedules.
	/// This includes counters and date-related fields necessary for computations,
	/// effectively clearing any prior state to ensure a clean slate for new calculations.
	/// </summary>
	private void ResetCalculationDynamicProperties()
	{
		_currentWeekIndex = 0;
		_currentHomeOfficeCount = 0;
		_currentOfficeCount = 0;
		_lastUpdateUserSettings = DateTime.MinValue;
		_currentStartOfWeek = DateTime.MinValue;
	}

	/// <summary>
	/// Determines the color associated with a specific day type in the context of scheduling.
	/// The resulting color is returned as a hexadecimal string for visual representation.
	/// </summary>
	private string GetWeekDayColor(DayType dayType) =>
		dayType switch
		{
			DayType.HOME => "#003764",
			DayType.OFFICE => "#357a32",
			_ => "#FFFFFF"
		};

	/// <summary>
	/// Creates a new instance of <see cref="WeekDayModel"/> populated with details for the specified day.
	/// The model includes the day type, date, and a corresponding hexadecimal color value based on the type.
	/// </summary>
	/// <param name="dayType">The type of the day, representing whether it is a home or office day.</param>
	/// <param name="date">The specific date associated with the day being modeled.</param>
	/// <returns>An instance of <see cref="WeekDayModel"/> containing the provided day details.</returns>
	private WeekDayModel CreateWeekDayModel(DayType dayType, DateTime date) =>
		new()
		{
			Type = dayType,
			Date = date,
			HexColor = GetWeekDayColor(dayType)
		};

	/// <summary>
	/// Asynchronously calculates a collection of week models based on user settings and pre-defined options.
	/// The calculation process resets relevant properties, retrieves user settings,
	/// and iteratively computes week details up to a defined limit.
	/// </summary>
	/// <returns>
	/// An array of <see cref="CalculatedWeekModel"/> objects representing the calculated weeks,
	/// or <c>null</c> if user settings could not be retrieved.
	/// </returns>
	public async Task<CalculatedWeekModel[]?> CalculateWeeksAsync()
	{
		ResetCalculationDynamicProperties();

		var us = await _databaseService.GetUserSettingAsync();
		if (us is null)
		{
			_logService.Error("User settings could not be retrieved.");
			return null;
		}

		_currentHomeOfficeCount = us.HomeOfficeDayCount;
		_currentOfficeCount = us.OfficeDayCount;
		_homeOfficeTargetQuoted = us.HomeOfficeTargetQuoted;
		_officeTargetQuoted = us.OfficeTargetQuoted;
		_lastUpdateUserSettings = us.LastUpdate;
		_currentStartOfWeek = DateTimeHelper.GetStartOfWeek(_lastUpdateUserSettings);

		List<CalculatedWeekModel> cwsTotal = new();

		for (int i = 0; i < Options.CALCULATE_WEEKS_COUNT; i++)
		{
			var cw = await CalculateNextWeekAsync();
			if (cw is not null)
			{
				cw.HomeOfficeTargetQuoted = _homeOfficeTargetQuoted;
				cw.OfficeTargetQuoted = _officeTargetQuoted;
				cwsTotal.Add(cw);
			}
		}

		return cwsTotal.ToArray();
	}

	/// <summary>
	/// Asynchronously calculates the details of the next week, including start and end dates,
	/// distribution of home office days, office days, and other related properties.
	/// Constructs a new instance of <see cref="CalculatedWeekModel"/> based on the provided runtime data,
	/// keeping track of the total counts and creating a structured representation for the week.
	/// </summary>
	/// <returns>
	/// A <see cref="CalculatedWeekModel"/> object representing the calculated details of the next week,
	/// or null if an error occurs during the calculation.
	/// </returns>
	public async Task<CalculatedWeekModel?> CalculateNextWeekAsync()
	{
		try
		{
			// TODO: add check for plannable day entrys

			// Set the start and end date of the current week + increase the current week index
			_currentStartOfWeek = _currentStartOfWeek.AddDays(7);
			_currentWeekIndex++;

			var homeOfficeDays = _mainWindowService.RuntimeData.HomeOfficeDays;
			var officeDays = _mainWindowService.RuntimeData.OfficeDays;

			var dayOfWeeks = homeOfficeDays
				.Select(day => new { Day = day, Type = DayType.HOME })
				.Concat(officeDays.Select(day => new { Day = day, Type = DayType.OFFICE }))
				.OrderBy(item => item.Day)
				.ToArray();

			_currentHomeOfficeCount += (uint)homeOfficeDays.Length;
			_currentOfficeCount += (uint)officeDays.Length;

			var weekDayStart = _currentStartOfWeek.AddDays(-1);
			var weekDays = new List<WeekDayModel>();
			foreach (var item in dayOfWeeks)
			{
				weekDayStart = weekDayStart.AddDays(1);
				var wd = CreateWeekDayModel(item.Type, weekDayStart);
				weekDays.Add(wd);
			}

			var cw = new CalculatedWeekModel()
			{
				WeekName = $"Woche {_currentWeekIndex}",
				HomeOfficeTargetQuoted = _homeOfficeTargetQuoted,
				OfficeTargetQuoted = _officeTargetQuoted,
				HomeOfficeDays = _currentHomeOfficeCount,
				OfficeDays = _currentOfficeCount,
				WeekDays = weekDays.ToArray()
			};

			return cw;
		}
		catch (Exception e)
		{
			_logService.Error(e.Message);
		}

		return null;
	}
}
