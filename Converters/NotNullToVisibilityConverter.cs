namespace OfficeTracker.Converters;

/// Converts a non-null object to a visibility-related value.
/// This is typically used in UI frameworks to determine visibility
/// based on whether the provided object is null or not.
/// Implements the `IValueConverter` interface.
public sealed class NotNullToVisibilityConverter : IValueConverter
{
	/// Converts a value to a boolean indicating its non-null state.
	public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => value != null;

	public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}
