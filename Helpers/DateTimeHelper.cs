namespace OfficeTracker.Helpers;

public static class DateTimeHelper
{
	public static bool IsToday(DateTime dt)
		=> dt.Date == DateTime.Today;
}
