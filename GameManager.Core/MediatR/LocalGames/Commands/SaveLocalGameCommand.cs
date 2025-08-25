using GameManager.Core.Data.Settings;

using Newtonsoft.Json.Linq;

namespace GameManager.Core.MediatR.LocalGames.Commands;

public record SaveLocalGameCommand(LocalGame Game) : IRequest<LocalGame>;

internal class SaveLocalGameCommandHandler : IRequestHandler<SaveLocalGameCommand, LocalGame>
{
    private readonly IFileRepo _fileRepo;
    private readonly ILocalGameRepo _gameRepo;

    public SaveLocalGameCommandHandler(ILocalGameRepo gameRepo, IFileRepo fileRepo)
    {
        _fileRepo = fileRepo;
        _gameRepo = gameRepo;
    }

    public async Task<LocalGame> Handle(SaveLocalGameCommand request, CancellationToken cancellationToken)
    {
        await _gameRepo.SaveGame(request.Game, cancellationToken);
        CreateGameFolders(request.Game);

        return request.Game;
    }

    private void CreateGameFolders(LocalGame game)
    {
        var archiveFolder = Path.Combine(game.FullPath, SettingsConsts.ArchivesFolderName);
        var gameFolder = Path.Combine(game.FullPath, SettingsConsts.GameFolderName);
        var modsFolder = Path.Combine(game.FullPath, SettingsConsts.ModsFolderName);
        var savesFolder = Path.Combine(game.FullPath, SettingsConsts.SavesFolderName);

        _fileRepo.MakeFolderSafely(archiveFolder);
        _fileRepo.MakeFolderSafely(gameFolder);
        _fileRepo.MakeFolderSafely(modsFolder);
        _fileRepo.MakeFolderSafely(savesFolder);
    }
}
