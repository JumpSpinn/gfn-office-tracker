namespace OfficeTracker.Features.Dialog.Content;

public partial class RestartContentDialog : UserControl, INotifyPropertyChanged, IDisposable
{
	private TimingController? _timingController;
	private uint _countdown;
	private uint? _timerHandle;
	private Action? _onCountdownComplete;

	public RestartContentDialog() { }
	public RestartContentDialog(TimingController timingController) : this()
	{
		_timingController = timingController;
	}

	#region PROPERTIES

	public uint Countdown
	{
		get => _countdown;
		set
		{
			if (_countdown == value) return;
			_countdown = value;
			OnPropertyChanged();
		}
	}

	private string _description = string.Empty;
	public string Description
	{
		get => _description;
		set
		{
			if (_description == value) return;
			_description = value;
			OnPropertyChanged();
		}
	}

	private string _restartMessage = string.Empty;
	public string RestartMessage
	{
		get => _restartMessage;
		set
		{
			if (_restartMessage == value) return;
			_restartMessage = value;
			OnPropertyChanged();
		}
	}

	#endregion

	public void StartCountdown(Action onComplete)
	{
		_onCountdownComplete = onComplete;
		_timerHandle = _timingController?.SetInterval(
			"RestartCountdown",
			OnCountdownTick,
			1_000,
			Countdown * 1_000
		);
	}

	public void StopCountdown()
	{
		if (!_timerHandle.HasValue) return;
		_timingController?.ClearInterval(_timerHandle);
		_timerHandle = null;
	}

	private void OnCountdownTick() =>
		Dispatcher.UIThread.Post(() =>
		{
			Countdown--;
			if (Countdown > 0) return;

			StopCountdown();
			_onCountdownComplete?.Invoke();
		});

	#region NOTIFY PROPERTY CHANGED

	public new event PropertyChangedEventHandler? PropertyChanged;

	private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
		=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

	#endregion

	#region DISPOSE

	public void Dispose()
	{
		StopCountdown();
		GC.SuppressFinalize(this);
	}

	#endregion
}

