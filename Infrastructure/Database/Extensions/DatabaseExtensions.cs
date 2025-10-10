namespace OfficeTracker.Infrastructure.Database.Extensions;

public static class DatabaseExtensions
{
	/// <summary>
	/// Extension method to add database services to the service collection.
	/// </summary>
	public static IServiceCollection AddDatabaseServices(this IServiceCollection services)
	{
		services.AddDbContext<OtContext>(options =>
		{
#if DEBUG
			options
				.EnableSensitiveDataLogging()
				.EnableDetailedErrors()
				.LogTo(Console.WriteLine, LogLevel.Information);
#else
	            options.EnableDetailedErrors();
#endif
		}, ServiceLifetime.Transient);

		services.AddSingleton<IDbContextFactory<OtContext>, OtContextFactory>();

		return services;
	}
}
