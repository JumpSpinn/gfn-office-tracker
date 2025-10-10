namespace OfficeTracker.Core.Services.Time.Entities;

public sealed class TimerEntity
{
	public Timer Timer { get; }
	public string Name { get; }
	public TimerType Type { get; }

	public TimerEntity(Timer timer, string name, TimerType type)
	{
		Timer = timer;
		Name = name;
		Type = type;
	}
}
