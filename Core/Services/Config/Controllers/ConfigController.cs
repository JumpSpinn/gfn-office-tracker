namespace OfficeTracker.Core.Services.Config.Controllers;

/// <summary>
/// The ConfigController class is responsible for managing the application's configuration,
/// including initializing, storing, and saving configurations.
/// It acts as an intermediary between the LogService, ConfigService, and the application's configuration data.
/// This class is sealed to prevent inheritance.
/// </summary>
[RegisterSingleton]
public sealed class ConfigController
{
	private readonly LogService _logService;
	private readonly ConfigService _configService;

	public ConfigController(LogService ls, ConfigService cs)
	{
		_logService = ls;
		_configService = cs;
	}

	/// <summary>
	/// Gets the instance of the application's local configuration.
	/// Allows retrieval and updates to the current configuration.
	/// Provides property change notifications when the configuration is updated.
	/// </summary>
	public LocalConfig Config { get; private set; } = new();

	/// <summary>
	/// Asynchronously saves the current configuration to a file.
	/// Delegates the actual saving process to the ConfigService.
	/// </summary>
	public async Task<bool> SaveConfigToFile()
		=> await _configService.SaveConfigToFileAsync(Config);

	/// <summary>
	/// Asynchronously initializes the configuration by attempting to load it from a file.
	/// If the configuration file does not exist or cannot be loaded, a default configuration is created
	/// and saved. The configuration is then assigned to the controller's Config property.
	/// Logs relevant information and exceptions during the process.
	/// </summary>
	public async Task<bool> InitializeConfigAsync()
	{
		try
		{
			var config = await _configService.LoadConfigFromFileAsync();
			if (config is null)
			{
				_logService.Info("Config file could not be loaded, creating dummy config.");
				return await SaveConfigToFile();
			}

			Config.WindowSize = config.WindowSize;
			Config.WindowPosition = config.WindowPosition;
			Config.RememberWindowPositionSize = config.RememberWindowPositionSize;
			Config.Language = config.Language;

			return await SaveConfigToFile();
		}
		catch (Exception e)
		{
			_logService.Exception(e);
		}

		return false;
	}
}
