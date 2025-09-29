namespace OfficeTracker.Converters.Models;

/// <summary>
/// Represents a pair of colors associated with a plannable day entity.
/// This class provides properties for both background and border colors,
/// allowing customization of the appearance for different day types.
/// </summary>
public class PlannableDayColorPair(string backgroundColor, string borderColor)
{
	public IBrush Background { get; set; }
		= new SolidColorBrush(Color.Parse(backgroundColor));

	public IBrush Border { get; set; }
		= new SolidColorBrush(Color.Parse(borderColor));
}
