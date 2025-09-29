namespace OfficeTracker.Helpers;

/// <summary>
/// Utility class providing helper methods for common operations related to DateTime.
/// </summary>
public static class DateTimeHelper
{
	/// <summary>
	/// Determines whether the specified date is today's date.
	/// </summary>
	/// <param name="dt">The date to evaluate.</param>
	/// <returns>True if the specified date is today's date; otherwise, false.</returns>
	public static bool IsToday(DateTime dt)
		=> dt.Date == DateTime.Today;

	/// <summary>
	/// Determines whether the specified date is in the past, relative to today's date.
	/// </summary>
	/// <param name="dt">The date to evaluate.</param>
	/// <returns>True if the specified date is in the past; otherwise, false.</returns>
	public static bool IsInPast(DateTime dt)
		=> dt.Date < DateTime.Today;

	/// <summary>
	/// Determines whether the specified date falls on a weekend.
	/// </summary>
	/// <param name="dt">The date to evaluate.</param>
	/// <returns>True if the specified date is a Saturday or Sunday; otherwise, false.</returns>
	public static bool IsInWeekend(DateTime dt)
		=> dt.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;

	/// <summary>
	/// Determines whether the specified date falls on a home office day.
	/// </summary>
	/// <param name="dt">The date to evaluate.</param>
	/// <returns>True if the specified date falls on a home office day; otherwise, false.</returns>
	public static bool IsHomeOfficeDay(DateTime dt)
		=> Options.HomeOfficeDays.Contains(dt.DayOfWeek);

	/// <summary>
	/// Determines whether the specified date is a designated office day.
	/// </summary>
	/// <param name="dt">The date to evaluate.</param>
	/// <returns>True if the specified date is an office day; otherwise, false.</returns>
	public static bool IsOfficeDay(DateTime dt)
		=> Options.OfficeDays.Contains(dt.DayOfWeek);
}
