namespace OfficeTracker.Converters;

/// Provides functionality to convert `DayType` enumeration values to descriptive strings
/// and optionally convert such strings back to the corresponding enumeration values.
/// Implements the `IValueConverter` interface to support data binding in UI frameworks.
public sealed class PlannableDayTypeConverter : IValueConverter
{
	/// Converts an instance of the `DayType` enum to a corresponding descriptive string.
	/// Non-`DayType` inputs are converted to a default string value "Unbekannt".
	public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		if (value is not DayType type)
			return "Unbekannt";

		return type switch
		{
			DayType.HOME => "HomeOffice",
			DayType.OFFICE => "Standort",
			_ => "Nicht festgelegt"
		};
	}

	public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
		=> throw new NotImplementedException();
}
