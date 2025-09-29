namespace OfficeTracker.Helpers;

public static class DateTimeHelper
{
	public static bool IsToday(DateTime dt)
		=> dt.Date == DateTime.Today;

	public static bool IsInPast(DateTime dt)
		=> dt.Date < DateTime.Today;

	public static bool IsInWeekend(DateTime dt)
		=> dt.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;
}
