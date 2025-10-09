namespace OfficeTracker.Infrastructure.Database;

/*
 * KAIWO ALLA:
 * du könntest das mit der db auch machen wie bei minty , wir speichern das in der
 * appdata unter nem ordner der minty heißt dort liegt dann alles zur exe weil sonst
 * ist es kacke wenn die leute die exe aufn desktop packen etc , man könnte im programm
 * dann oben eine "info" knopf oder nen "?" machen und damit öffnest du dann den dateipfad
 * falls du zu dem ordner willst , sollte eig ganz nice sein
 */

/// <summary>
/// Represents the database context for the OfficeTracker application,
/// configured to manage and interact with database models and operations.
/// </summary>
public sealed partial class OtContext(DbContextOptions<Infrastructure.Database.OtContext> options) : DbContext(options)
{
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		=> base.OnConfiguring(optionsBuilder);

	protected override void OnModelCreating(ModelBuilder modelBuilder)
		=> base.OnModelCreating(modelBuilder);
}
