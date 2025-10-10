namespace OfficeTracker.Core.Services.Time.Controllers;

[RegisterSingleton]
public sealed class TimingController(LogController logController)
{
	private readonly ConcurrentDictionary<uint, TimerEntity> _timers = new();
	private readonly ConcurrentDictionary<uint, string> _intervals = new();
	private readonly ConcurrentDictionary<uint, string> _timeouts = new();
	private uint _nextHandle = 1;
	private readonly object _handleLock = new();

	public IReadOnlyDictionary<uint, string> Intervals => _intervals;
	public IReadOnlyDictionary<uint, string> Timeouts => _timeouts;

	#region INTERVAL

	public uint SetInterval(string name, Action action, uint timeMs, uint durationMs = 0)
	{
		var handle = GetNextHandle();
		var timer = new Timer(_ =>
		{
			try
			{
				action();
			}
			catch (Exception ex)
			{
				logController.Error($"Error in interval '{name}' (Handle: {handle}) - {ex.Message}");
			}
		}, null, timeMs, timeMs);

		_timers[handle] = new TimerEntity(timer, name, TimerType.Interval);
		_intervals[handle] = name;

		if (durationMs > 0)
		{
			SetTimeout($"{name} - Duration", () => ClearInterval(handle), durationMs);
		}

		return handle;
	}

	public void ClearInterval(uint? handle)
	{
		if (handle == null) return;

		try
		{
			if (!_timers.TryRemove(handle.Value, out var entry)) return;
			entry.Timer.Dispose();
			_intervals.TryRemove(handle.Value, out _);
		}
		catch (Exception ex)
		{
			logController.Error($"Error clearing interval (Handle: {handle}) - {ex.Message}");
		}
	}

	public void ClearInterval(ref uint? handle)
	{
		if (handle == null) return;
		ClearInterval(handle);
		handle = null;
	}

	#endregion

	#region TIMEOUT

	public uint SetTimeout(string name, Action action, uint timeMs)
	{
		var handle = GetNextHandle();
		Timer? timer;

		timer = new Timer(_ =>
		{
			try
			{
				action();
			}
			catch (Exception ex)
			{
				logController.Error($"Error in timeout '{name}' (Handle: {handle}) - {ex.Message}");
			}
			finally
			{
				_timers.TryRemove(handle, out var entry);
				entry?.Timer.Dispose();
				_timeouts.TryRemove(handle, out var _);
			}
		}, null, timeMs, Timeout.Infinite);

		_timers[handle] = new TimerEntity(timer, name, TimerType.Timeout);
		_timeouts[handle] = name;

		return handle;
	}

	public void ClearTimeout(uint? handle)
	{
		if (handle == null) return;

		try
		{
			if (!_timers.TryRemove(handle.Value, out var entry)) return;
			entry.Timer.Dispose();
			_timeouts.TryRemove(handle.Value, out _);
		}
		catch (Exception ex)
		{
			logController.Error($"Error clearing timeout (Handle: {handle}) - {ex.Message}");
		}
	}

	#endregion

	#region DELAY

	public async Task Delay(uint ms)
		=> await Task.Delay((int)ms);

	#endregion

	#region WATCH

	public void StartWatch(WatchEntity watch)
	{
		if (watch.IsRunning) return;

		watch.Ticks = watch.StartTicks;
		watch.Interval = SetInterval($"Watch {watch.Id}", () =>
		{
			watch.Ticks++;
			if (watch.Action == null) return;
			if (watch.Ticks < watch.TargetTicks) return;

			logController.Debug($"Watch {watch.Id} elapsed");
			StopWatch(watch);
			watch.Action.Invoke();
		}, watch.Precision);
	}

	public void RestartWatch(WatchEntity watch)
	{
		ResetWatch(watch);
		StartWatch(watch);
	}

	public uint ResetWatch(WatchEntity watch)
	{
		var elapsed = StopWatch(watch);
		watch.Ticks = watch.StartTicks;
		return elapsed;
	}

	public uint StopWatch(WatchEntity watch)
	{
		ClearInterval(watch.Interval);
		watch.Interval = null;
		return watch.Value;
	}

	#endregion

	#region HELPERS

	private uint GetNextHandle()
	{
		lock (_handleLock)
			return _nextHandle++;
	}

	public void Dispose()
	{
		foreach (var entry in _timers.Values)
		{
			try
			{
				entry.Timer.Dispose();
			}
			catch (Exception ex)
			{
				logController.Error($"Error disposing timer '{entry.Name}' - {ex.Message}");
			}
		}
		_timers.Clear();
		_intervals.Clear();
		_timeouts.Clear();
	}

	#endregion
}
