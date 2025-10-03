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
	public static bool IsInWeekend(DateTime dt)
		=> dt.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;

	/// <summary>
	/// Determines whether a specified date falls on a day contained within the given array of days.
	/// </summary>
	public static bool IsDateInDayArray(DateTime dt, DayOfWeek[] weekDays)
		=> weekDays.Contains(dt.DayOfWeek);
}
