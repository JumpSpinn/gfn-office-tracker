namespace OfficeTracker.Core.Entities;

/// <summary>
/// Represents a configuration vector consisting of two integer components, X and Y.
/// </summary>
public sealed class ConfigIntVectorEntity(int x, int y)
{
	public int X { get; set; } = x;
	public int Y { get; set; } = y;
}
