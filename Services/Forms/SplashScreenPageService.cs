namespace OfficeTracker.Services.Forms;

[RegisterSingleton]
public sealed class SplashScreenPageService(IDbContextFactory<OfContext> dbContextFactory)
{
	private readonly IDbContextFactory<OfContext> _dbContextFactory = dbContextFactory;

	// TODO: check if database is initialized
}
