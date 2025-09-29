namespace OfficeTracker.Converters.Models;

public class PlannableDayColorPair(string backgroundColor, string borderColor)
{
	public IBrush Background { get; set; }
		= new SolidColorBrush(Color.Parse(backgroundColor));

	public IBrush Border { get; set; }
		= new SolidColorBrush(Color.Parse(borderColor));
}
