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

		var cookieContainer = new System.Net.CookieContainer();
		var handler = new HttpClientHandler
		{
			CookieContainer = cookieContainer,
			UseCookies = true,
			AllowAutoRedirect = true // Wichtig, falls die Seite nach dem Login weiterleitet
		};
		_httpClient = new HttpClient(handler);
	}

	public async Task<LoginResult?> RequestLoginAsync(string username, string password)
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

			return await PerformLoginAsync(username, password);
		}
		catch (Exception ex)
		{
			_logController.Exception(ex);
		}
		return null;
	}

	private async Task<LoginResult> PerformLoginAsync(string username, string password)
    {
        const string loginUrl = "https://lernplattform.gfn.de/login/index.php";

        var formContent = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("username", username),
            new KeyValuePair<string, string>("password", password)
        });

        try
        {
            _logController.Debug($"Sending POST request to {loginUrl} with credentials...");

            var response = await _httpClient.PostAsync(loginUrl, formContent);
            _logController.Debug("POST request sent.", response.ToString());

            // 3. Überprüfung des Response
            // Ein erfolgreicher Login führt oft zu einer Weiterleitung (Status 302)
            // und einem 'OK' (Status 200) auf der Zielseite.
            // Wir prüfen, ob die Antwort erfolgreich war (Status 200-299)
            if (response.IsSuccessStatusCode)
            {
                // Um sicherzustellen, dass der Login erfolgreich war,
                // sollte man den Inhalt der Antwortseite überprüfen.
                // Zum Beispiel, ob die Seite einen Hinweis enthält, dass man eingeloggt ist,
                // oder ob sie NICHT die Login-Fehlermeldung enthält.
                var responseContent = await response.Content.ReadAsStringAsync();

                // BEISPIEL: Prüfen, ob die URL immer noch die Login-Seite ist (bei Fehler)
                // oder ob eine typische Fehlermeldung enthalten ist.
                if (responseContent.Contains("Ungültiger Login")) // Dies ist ein Platzhalter!
                {
                    _logController.Error("GFN Login failed: Invalid credentials or token.");
                    return new LoginResult(false, "Login fehlgeschlagen. Überprüfen Sie Ihre Zugangsdaten.");
                }

                _logController.Debug("GFN Login successful.");
                return new LoginResult(true, null);
            }
            else
            {
                _logController.Error($"GFN Login failed with HTTP status: {response.StatusCode}");
                return new LoginResult(false, $"Login fehlgeschlagen. Serverantwort: {response.ReasonPhrase}");
            }
        }
        catch (HttpRequestException ex)
        {
            _logController.Exception(ex, "HTTP Request Error during GFN Login.");
            return new LoginResult(false, "Verbindungsfehler zur Lernplattform.");
        }
    }
}

public record LoginResult(bool Success, string? Message);
