namespace OfficeTracker.Converters;

public sealed class EmptyToVisibilityConverter : IValueConverter
{
	public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => (value as ICollection)!.Count == 0;

	public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}
