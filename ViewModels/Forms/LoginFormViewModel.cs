namespace OfficeTracker.ViewModels.Forms;

[RegisterSingleton]
public sealed partial class LoginFormViewModel : ViewModelBase
{
	private readonly LogController _logController;
	private readonly IServiceProvider _serviceProvider;

	public LoginFormViewModel(LogController lc, IServiceProvider sp)
	{
		_logController = lc;
		_serviceProvider = sp;
	}

	#region INFO BAR

	[ObservableProperty]
	private bool _showInfoBar;

	[ObservableProperty]
	private string _infoBarTitle = string.Empty;

	[ObservableProperty]
	private string _infoBarText = string.Empty;

	[ObservableProperty]
	private InfoBarSeverity _infoBarSeverity = InfoBarSeverity.Error;

	#endregion

	#region LOGIN BUTTON

	[ObservableProperty]
	private bool _isLoginButtonEnabled = true;

	[ObservableProperty]
	private string _loginButtonText = "Einloggen";

	#endregion
}
