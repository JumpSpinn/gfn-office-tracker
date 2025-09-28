namespace OfficeTracker.Services.Login.Controllers;

using System.Net.Http;

[RegisterSingleton]
public sealed class LoginController
{
	private readonly HttpClient _httpClient;
	private readonly LogController _logController;

	public LoginController(LogController lc)
	{
		_logController = lc;
	}

	public async Task<LoginResult?> PeformLoginAsync(string username, string password)
	{
		try
		{
			_logController.Debug($"Try to perform login with username: {username} ..");

			if (string.IsNullOrEmpty(username))
			{
				_logController.Error("E-Mail is empty.");
				return new LoginResult(false, "E-Mail darf nicht leer sein.");
			}

			if (!username.Contains('@'))
			{
				_logController.Error("Username is not a valid E-Mail.");
				return new LoginResult(false, "E-Mail ist ungültig.");
			}

			if (string.IsNullOrEmpty(password))
			{
				_logController.Error("Password is empty.");
				return new LoginResult(false, "Passwort darf nicht leer sein.");
			}

			// TODO: peform login to gfn alla

			return new LoginResult(true, null);
		}
		catch (Exception ex)
		{
			_logController.Exception(ex);
		}
		return null;
	}
}

public record LoginResult(bool Success, string? Message);
