using GameManager.Core.Data.Settings;


namespace GameManager.Core.Migrations;

public class GameFolderStructureMigration : IGameManagerMigration
{
    private readonly ILocalGameRepo _localGameRepo;
    private readonly IFileRepo _fileRepo;
    private readonly ILogger<GameFolderStructureMigration> _logger;

    public GameFolderStructureMigration(ILocalGameRepo localGameRepo, IFileRepo fileRepo)
    {
        _localGameRepo = localGameRepo;
        _fileRepo = fileRepo;
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
                MoveArchiveFilesToNewArchiveFolder(game);
                MoveExtraFoldersToNewGameFolder(game);
                MakeSavesFolder(game);
                MakeModsFolderGame(game);
            }
            catch ( Exception ex )
            {
                _logger.LogError(ex, $"Error migrating game {game.FullPath}");
                throw;
            }
        }
    }

    internal LocalGame MoveArchiveFilesToNewArchiveFolder(LocalGame game)
    {
        var archiveFolder = Path.Combine(game.FullPath, SettingsConsts.ArchivesFolderName);
        _fileRepo.MakeFolderSafely(archiveFolder);

        var archiveFilePaths = _fileRepo.GetArchiveFilePathsInFolder(game.FullPath, true);

        foreach ( var archiveFilePath in archiveFilePaths )
        {
            var archiveFileName = Path.GetFileName(archiveFilePath);
            var newArchiveFilePath = Path.Combine(archiveFolder, archiveFileName);
            File.Move(archiveFilePath, newArchiveFilePath);
        }

        return game;
    }

    internal LocalGame MoveExtraFoldersToNewGameFolder(LocalGame game)
    {
        //ignoring the archive, mods and saves directories move all other folders to the new game folder
        var gameFolder = Path.Combine(game.FullPath, SettingsConsts.GameFolderName);
        _fileRepo.MakeFolderSafely(gameFolder);

        var foldersToIgnore = new List<string> {
            SettingsConsts.GameFolderName,
            SettingsConsts.ArchivesFolderName,
            SettingsConsts.ModsFolderName,
            SettingsConsts.SavesFolderName
        };

        //GetFileName returns the last part of the directory
        var foldersToMove = Directory.GetDirectories(game.FullPath)
            .Where(_ => !foldersToIgnore.Contains(Path.GetFileName(_) ?? throw new InvalidOperationException()));

        foreach ( var folderToMove in foldersToMove )
        {
            var newPath = Path.Combine(gameFolder, Path.GetFileName(folderToMove));
            Directory.Move(folderToMove, newPath);
        }

        return game;
    }

    internal void MakeModsFolderGame(LocalGame game)
    {
        var modsFolder = Path.Combine(game.FullPath, SettingsConsts.ModsFolderName);
        _fileRepo.MakeFolderSafely(modsFolder);
    }

    internal void MakeSavesFolder(LocalGame game)
    {
        var savesFolder = Path.Combine(game.FullPath, SettingsConsts.SavesFolderName);
        _fileRepo.MakeFolderSafely(savesFolder);
    }
}