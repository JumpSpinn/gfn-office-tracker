namespace OfficeTracker.Core.Services.Time.Entities;

public sealed class WatchEntity
{
	public string Id { get; set; } = Guid.NewGuid().ToString();
	public uint Ticks { get; set; }
	public uint StartTicks { get; set; }
	public uint TargetTicks { get; set; }
	public uint Precision { get; set; } = 100;
	public uint? Interval { get; set; }
	public Action? Action { get; set; }

	public bool IsRunning => Interval.HasValue;
	public uint Value => (Ticks - StartTicks) * Precision;
}
