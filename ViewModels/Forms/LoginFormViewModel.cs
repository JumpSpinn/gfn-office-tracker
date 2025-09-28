namespace OfficeTracker.ViewModels.Forms;

[RegisterSingleton]
public sealed partial class LoginFormViewModel : ViewModelBase
{
	private readonly LogController _logController;
	private readonly LoginController _loginController;
	private readonly IServiceProvider _serviceProvider;

	public LoginFormViewModel(LogController lc, IServiceProvider sp, LoginController lC)
	{
		_logController = lc;
		_serviceProvider = sp;
		_loginController = lC;
	}

	#region LOGIN

	[ObservableProperty]
	[NotifyCanExecuteChangedFor(nameof(LoginCommand))]
	[NotifyPropertyChangedFor(nameof(CanLoginProperty))]
	private bool _isLoginButtonEnabled = true;

	[ObservableProperty]
	private string _loginButtonText = "Statistik berechnen";

	[ObservableProperty]
	[NotifyCanExecuteChangedFor(nameof(LoginCommand))]
	[NotifyPropertyChangedFor(nameof(CanLoginProperty))]
	private string _username = string.Empty;

	[ObservableProperty]
	[NotifyCanExecuteChangedFor(nameof(LoginCommand))]
	[NotifyPropertyChangedFor(nameof(CanLoginProperty))]
	private string _password = string.Empty;

	private bool CanLogin()
		=> !string.IsNullOrWhiteSpace(Username) &&
		   !string.IsNullOrWhiteSpace(Password) &&
		   IsLoginButtonEnabled;

	public bool CanLoginProperty
		=> !string.IsNullOrWhiteSpace(Username) &&
		   !string.IsNullOrWhiteSpace(Password) &&
		   IsLoginButtonEnabled;

	[RelayCommand(CanExecute = nameof(CanLogin))]
	private async Task LoginAsync()
	{
		ResetInfoBar();
		IsLoginButtonEnabled = false;
		LoginButtonText = "Bitte warten..";

		try
		{
			var result = await _loginController.RequestLoginAsync(Username, Password);
			if (result is null)
			{
				_logController.Error("Login result is null.");
			}
			else
			{
				if (!result.Success)
				{
					InfoBarTitle = "Ungültiger Login";
					InfoBarText = result.Message ?? "Unbekannter Fehler";
					InfoBarSeverity = InfoBarSeverity.Error;
					ShowInfoBar = true;
				}
				else
				{
					InfoBarTitle = "Erfolgreicher Login";
					InfoBarText = "Juhu wir haben es geschafft KEULE ALLA";
					InfoBarSeverity = InfoBarSeverity.Success;
					ShowInfoBar = true;
				}
			}
		}
		catch (Exception e)
		{
			_logController.Error($"Error on login. Details: {e.Message}");
		}
		finally
		{
			ResetLoginForm();
		}
	}

	private void ResetLoginForm()
	{
		Username = "";
		Password = "";
		LoginButtonText = "Einloggen";
		IsLoginButtonEnabled = true;
	}

	#endregion

	#region INFO BAR

	[ObservableProperty]
	private bool _showInfoBar;

	[ObservableProperty]
	private string _infoBarTitle = string.Empty;

	[ObservableProperty]
	private string _infoBarText = string.Empty;

	[ObservableProperty]
	private InfoBarSeverity _infoBarSeverity = InfoBarSeverity.Error;

	private void ResetInfoBar()
	{
		ShowInfoBar = false;
		InfoBarTitle = "";
		InfoBarText = "";
		InfoBarSeverity = InfoBarSeverity.Error;
	}

	#endregion
}
