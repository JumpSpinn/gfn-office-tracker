namespace OfficeTracker.Infrastructure.Database.Extensions;

public static class DatabaseExtensions
{
	/// <summary>
	/// Extension method to add database services to the service collection.
	/// </summary>
	public static IServiceCollection AddDatabaseServices(this IServiceCollection services)
	{
		services.AddDbContext<OtContext>(ServiceLifetime.Transient);
		services.AddSingleton<IDbContextFactory<OtContext>, OtContextFactory>();
		return services;
	}
}
