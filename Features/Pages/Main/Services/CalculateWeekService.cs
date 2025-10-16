namespace OfficeTracker.Features.Pages.Main.Services;

/// <summary>
/// Service responsible for calculating and managing weekly schedules based on user settings
/// and other dynamic properties. It performs operations to compute upcoming weeks
/// and tracks relevant week data.
/// </summary>
[RegisterSingleton]
public sealed class CalculateWeekService
{
	private readonly LogController _logController;
	private readonly DatabaseService _databaseService;
	private readonly MainWindowController _mainWindowController;

	public CalculateWeekService(LogController ls, DatabaseService ds, MainWindowController mws)
	{
		_logController = ls;
		_databaseService = ds;
		_mainWindowController = mws;
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
			DayType.PASS => "#575757",
			_ => "#FFFFFF"
		};

	/// <summary>
	/// Creates a new instance of <see cref="WeekDayEntity"/> populated with details for the specified day.
	/// The model includes the day type, date, and a corresponding hexadecimal color value based on the type.
	/// </summary>
	private WeekDayEntity CreateWeekDayModel(DayType dayType, DateTime date) =>
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
	public async Task<CalculatedWeekEntity[]?> CalculateWeeksAsync()
	{
		ResetCalculationDynamicProperties();

		var us = await _databaseService.GetUserSettingAsync();
		if (us is null)
		{
			_logController.Error("User settings could not be retrieved.");
			return null;
		}

		_currentHomeOfficeCount = us.HomeOfficeDayCount;
		_currentOfficeCount = us.OfficeDayCount;
		_homeOfficeTargetQuoted = us.HomeOfficeTargetQuoted;
		_officeTargetQuoted = us.OfficeTargetQuoted;

		// Check if we have a nice date to start from
		_lastUpdateUserSettings = (us.LastUpdate == DateTime.MinValue ? DateTime.Today : us.LastUpdate);
		_currentStartOfWeek = DateTimeHelper.GetStartOfCurrentWeek(_lastUpdateUserSettings);

		List<CalculatedWeekEntity> cwsTotal = new();

		// calculate current week
		var currentCalculatedWeek = await CalculateCurrentWeekAsync();
		if(currentCalculatedWeek is not null)
			cwsTotal.Add(currentCalculatedWeek);

		for (int i = 0; i < Options.CALCULATE_WEEKS_COUNT; i++)
		{
			var cw = await CalculateNextWeekAsync();
			if (cw is null) continue;

			cw.HomeOfficeTargetQuoted = _homeOfficeTargetQuoted;
			cw.OfficeTargetQuoted = _officeTargetQuoted;
			cwsTotal.Add(cw);
		}

		return cwsTotal.ToArray();
	}

	/// <summary>
	/// Calculates the specified number of weekly schedules asynchronously.
	/// This method generates an array of calculated weekly data by repeatedly
	/// invoking the logic for computing the next week based on predefined settings.
	/// </summary>
	public async Task<CalculatedWeekEntity[]?> CalculateWeeksCountAsync(decimal weeksToCalculate)
	{
		try
		{
			List<CalculatedWeekEntity> cwsTotal = new();

			for (int i = 0; i < weeksToCalculate; i++)
			{
				var cw = await CalculateNextWeekAsync();
				if (cw is not null)
					cwsTotal.Add(cw);
			}

			return cwsTotal.ToArray();
		}
		catch (Exception e)
		{
			_logController.Exception(e);
		}
		return null;
	}

	/// <summary>
	/// Asynchronously calculates the details of the next week, including start and end dates,
	/// distribution of home office days, office days, and other related properties.
	/// Constructs a new instance of <see cref="CalculatedWeekEntity"/> based on the provided runtime data,
	/// keeping track of the total counts and creating a structured representation for the week.
	/// </summary>
	public async Task<CalculatedWeekEntity?> CalculateNextWeekAsync()
	{
		try
		{
			// Set the start and end date of the current week + increase the current week index
			_currentStartOfWeek = _currentStartOfWeek.AddDays(7);
			_currentWeekIndex++;

			var weekDayStart = _currentStartOfWeek.AddDays(-1);
			var weekDays = new List<WeekDayEntity>();
			foreach (var item in _mainWindowController.RuntimeDataEntity.DayOfWeeks)
			{
				var dayType = item.Type;

				weekDayStart = weekDayStart.AddDays(1);
				var pd = await _databaseService.GetSinglePlannableDayAsync(weekDayStart);
				if (pd is not null)
					dayType = pd.Type;

				switch (dayType)
				{
					case DayType.HOME:
						_currentHomeOfficeCount++;
						break;
					case DayType.OFFICE:
						_currentOfficeCount++;
						break;
				}

				var wd = CreateWeekDayModel(dayType, weekDayStart);
				weekDays.Add(wd);
			}

			var cw = new CalculatedWeekEntity()
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
			_logController.Exception(e);
		}

		return null;
	}

	private async Task<CalculatedWeekEntity?> CalculateCurrentWeekAsync()
	{
		try
		{
			var startCurrentWeek = DateTimeHelper.GetStartOfCurrentWeek(DateTime.Today);
			var remainingDays = DateTimeHelper.GetRemainingDaysOfWeek();

			var weekDays = new List<WeekDayEntity>();
			foreach (var item in _mainWindowController.RuntimeDataEntity.DayOfWeeks)
			{
				var dayType = item.Type;
				var currentDay = remainingDays.FirstOrDefault(x => x.DayOfWeek == item.Day);
				if (currentDay == default)
				{
					startCurrentWeek = currentDay = startCurrentWeek.AddDays(1);
					dayType = DayType.PASS;
				}
				else
				{
					var pd = await _databaseService.GetSinglePlannableDayAsync(currentDay);
					if (pd is not null)
						dayType = pd.Type;

					switch (dayType)
					{
						case DayType.HOME:
							_currentHomeOfficeCount++;
							break;
						case DayType.OFFICE:
							_currentOfficeCount++;
							break;
					}
				}
				weekDays.Add(CreateWeekDayModel(dayType, currentDay));
			}

			var cw = new CalculatedWeekEntity()
			{
				WeekName = $"Aktuelle Woche",
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
			_logController.Exception(e);
		}

		return null;
	}
}
