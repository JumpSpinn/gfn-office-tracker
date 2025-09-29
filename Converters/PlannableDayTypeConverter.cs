namespace OfficeTracker.Converters;

public class PlannableDayTypeConverter : IValueConverter
{
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

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		=> throw new NotImplementedException();
}
