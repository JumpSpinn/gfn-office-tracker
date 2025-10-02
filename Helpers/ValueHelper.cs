namespace OfficeTracker.Helpers;

/// <summary>
/// Provides utility methods for handling and converting values.
/// </summary>
public static class ValueHelper
{
	/// <summary>
	/// Converts a nullable decimal value to its unsigned integer (uint) representation.
	/// If the given value is null or less than 0, the method returns 0.
	/// </summary>
	public static uint GetUintValue(decimal? value)
	{
		switch (value)
		{
			case null:
			case < 0:
				return 0;
			default:
				return (uint)value.Value;
		}
	}
}
