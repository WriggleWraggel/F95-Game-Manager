using GameManager.Core.Data;
using GameManager.Core.Data.Settings;


namespace GameManager.Core.Migrations;

public class GameUpdateAvailableMigration : IGameManagerMigration
{
    private readonly ILocalGameRepo _localGameRepo;
    private readonly ILogger<GameFolderStructureMigration> _logger;

    public GameUpdateAvailableMigration(ILocalGameRepo localGameRepo)
    {
        _localGameRepo = localGameRepo;
    }

    public int MigrationOrder => 1;
    public async Task ApplyMigration(AppSettings settings)
    {
        var cancellationToken = new CancellationToken();

        //find all games from root folders
        var games = await _localGameRepo.GameGamesInRootFolders(settings.GameLibrarySettings.Folders, cancellationToken);

        //for each game get the parent directory
        foreach ( var game in games )
        {
            try
            {
                game.UpdateAvailable = game.F95Game?.ThreadLastUpdatedDate > game.ArchiveFileLastUpdated;
                await _localGameRepo.SaveGame(game, CancellationToken.None);
            }
            catch ( Exception ex )
            {
                _logger.LogError(ex, $"Error migrating game {game.FullPath}");
                throw;
            }
        }
    }
}