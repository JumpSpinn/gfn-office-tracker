namespace OfficeTracker.Core.Converter;

/// Converts a null object to a visibility state.
/// This converter implements `IValueConverter` and is primarily used in XAML bindings
/// to translate null values into visibility indicators. It is particularly useful
/// in user interfaces to conditionally display elements.
/// Implements a unidirectional conversion from a nullable object to a visibility state,
/// where a null value typically corresponds to a hidden or collapsed state.
/// Throws `NotImplementedException` if the `ConvertBack` method is called, as
/// this converter is designed for one-way binding only.
public sealed class NullToVisibilityConverter : IValueConverter
{
	/// Converts a value to determine visibility based on whether the input value is null.
	public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => value == null;

	public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
		throw new NotImplementedException();
}
