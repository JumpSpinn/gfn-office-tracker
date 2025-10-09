namespace OfficeTracker.Core.Converter;

/// Converts an empty collection to a visibility state.
/// This class is used as a XAML converter to determine the visibility of UI elements
/// based on whether the provided collection is empty.
/// Implements the IValueConverter interface.
public sealed class EmptyToVisibilityConverter : IValueConverter
{
	/// Converts an input object to a visibility state based on whether the input is an empty collection or not.
	public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => (value as ICollection)!.Count == 0;

	public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}
