namespace OfficeTracker.Helpers;

public static class DateTimeHelper
{
	public static bool IsToday(DateTime dt)
		=> dt.Date == DateTime.Today;

	public static bool IsInPast(DateTime dt)
		=> dt.Date < DateTime.Today;

	public static bool IsInWeekend(DateTime dt)
		=> dt.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;

	public static bool IsHomeOfficeDay(DateTime dt)
		=> Options.HomeOfficeDays.Contains(dt.DayOfWeek);

	public static bool IsOfficeDay(DateTime dt)
		=> Options.OfficeDays.Contains(dt.DayOfWeek);
}
