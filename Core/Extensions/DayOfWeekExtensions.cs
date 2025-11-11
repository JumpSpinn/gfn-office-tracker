namespace OfficeTracker.Core.Extensions;

/// <summary>
/// Provides extension methods for the DayOfWeek enumeration.
/// </summary>
public static class DayOfWeekExtensions
{
	/// <summary>
	/// A dictionary that maps DayOfWeek values to their corresponding German abbreviations.
	/// </summary>
	/// <remarks>
	/// The German abbreviations are as follows:
	/// Monday -> "Mo", Tuesday -> "Di", Wednesday -> "Mi",
	/// Thursday -> "Do", Friday -> "Fr", Saturday -> "Sa", Sunday -> "So".
	/// This dictionary is used internally for converting DayOfWeek enums
	/// into their German abbreviations.
	/// </remarks>
	private static readonly Dictionary<DayOfWeek, string> _dayAbbreviations = new()
	{
		[DayOfWeek.Monday] = "Mo",
		[DayOfWeek.Tuesday] = "Di",
		[DayOfWeek.Wednesday] = "Mi",
		[DayOfWeek.Thursday] = "Do",
		[DayOfWeek.Friday] = "Fr",
		[DayOfWeek.Saturday] = "Sa",
		[DayOfWeek.Sunday] = "So"
	};

	/// <summary>
	/// Converts the specified DayOfWeek value to its German abbreviation.
	/// </summary>
	/// <param name="day">The DayOfWeek value to be converted.</param>
	/// <returns>
	/// The German abbreviation as a string corresponding to the given DayOfWeek value.
	/// If no abbreviation is found, returns the DayOfWeek value as a string.
	/// </returns>
	private static string ToGermanAbbreviation(this DayOfWeek day)
		=> _dayAbbreviations.GetValueOrDefault(day, day.ToString());

	/// <summary>
	/// Converts a collection of DayOfWeek values into a comma-separated string of their German abbreviations.
	/// </summary>
	/// <param name="days">The collection of DayOfWeek values to be converted.</param>
	/// <returns>
	/// A string containing the German abbreviations of the specified DayOfWeek values, separated by commas.
	/// If the collection is empty, returns an empty string.
	/// </returns>
	public static string ToCommaSeparatedString(this IEnumerable<DayOfWeek> days)
		=> string.Join(", ", days.Select(d => d.ToGermanAbbreviation()));
}
