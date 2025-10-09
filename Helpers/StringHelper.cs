namespace OfficeTracker.Helpers;

/// <summary>
/// Provides utility methods for working with strings.
/// </summary>
public static class StringHelper
{
	/// <summary>
	/// Truncates the specified string to a maximum length and appends an optional suffix.
	/// </summary>
	/// <param name="value">The string to be truncated.</param>
	/// <param name="maxLength">The maximum allowable length for the string. If the string is shorter than or equal to this length, it will not be truncated.</param>
	/// <param name="suffix">An optional suffix to append to the truncated string. Default is an empty string.</param>
	/// <returns>A string that is either the original input string, if its length is within the maximum length, or a truncated version with the suffix appended.</returns>
	public static string Truncate(this string value, int maxLength, string suffix = "")
	{
		if (string.IsNullOrEmpty(value) || value.Length <= maxLength)
			return value;

		return string.Concat(value.AsSpan(0, maxLength), suffix);
	}
}
