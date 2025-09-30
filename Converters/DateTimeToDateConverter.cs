namespace OfficeTracker.Converters;

public class DateTimeToDateConverter : IValueConverter
{
	public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		if (value is DateTime dateTime)
			return dateTime.ToString("dd.MM.yyyy");
		return value;
	}

	public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
