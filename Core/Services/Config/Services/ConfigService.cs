namespace OfficeTracker.Core.Services.Config.Services;

using Entities;

/// <summary>
/// Provides services for managing configuration files, including saving
/// and loading configuration data. Handles directory structure and file
/// operations required for persistent configuration storage.
/// </summary>
[RegisterSingleton]
public sealed class ConfigService
{
	private const string CONFIG_FILE_NAME = "config.json";

	/// <summary>
	/// Represents the directory path where configuration files are stored.
	/// It combines the application's data directory with the application domain's friendly name
	/// to construct a unique and consistent location for storing configuration data.
	/// </summary>
	private readonly string _configDirectory =
		Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppDomain.CurrentDomain.FriendlyName);

	/// <summary>
	/// Represents the full file path to the configuration file named "config.json".
	/// This is constructed by combining the directory path where configuration files are stored with the predefined configuration file name.
	/// </summary>
	private string ConfigFilePath =>
		Path.Combine(_configDirectory, CONFIG_FILE_NAME);

	/// <summary>
	/// Ensures that the configuration directory exists in the file system.
	/// If the directory does not already exist, it will attempt to create it.
	/// </summary>
	private bool EnsureConfigDirectoryExists()
		=> Directory.Exists(_configDirectory) || Directory.CreateDirectory(_configDirectory).Exists;

	/// <summary>
	/// Saves the provided configuration object to a file asynchronously.
	/// If the configuration directory does not exist, it will attempt to create it before saving the file.
	/// </summary>
	public async Task<bool> SaveConfigToFileAsync(ConfigEntity configEntity)
	{
		if (!EnsureConfigDirectoryExists()) return false;
		await File.WriteAllTextAsync(ConfigFilePath,
			JsonSerializer.Serialize(configEntity, Options.Config.JsonSerializerOptions));
		return true;
	}

	/// <summary>
	/// Asynchronously loads the configuration data from a JSON file.
	/// If the configuration directory or file does not exist, the method returns null.
	/// Deserializes the JSON file content into a LocalConfig object.
	/// </summary>
	public async Task<ConfigEntity?> LoadConfigFromFileAsync()
	{
		if (!EnsureConfigDirectoryExists()) return null;
		if(!File.Exists(ConfigFilePath)) return null;
		return JsonSerializer.Deserialize<ConfigEntity>(
			await File.ReadAllTextAsync(ConfigFilePath),
			Options.Config.JsonSerializerOptions
		);
	}
}
