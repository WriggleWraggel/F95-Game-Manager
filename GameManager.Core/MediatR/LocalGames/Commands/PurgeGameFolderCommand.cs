using GameManager.Core.Data.Settings;

namespace GameManager.Core.MediatR.LocalGames.Commands;

public record PurgeGameFoldersCommand(LocalGame Game, List<string> GameFolders) : IRequest<List<string>>;

internal class PurgeGameFoldersCommandHandler : IRequestHandler<PurgeGameFoldersCommand, List<string>>
{
    private readonly IFileRepo _fileRepo;
    private readonly ILogger<PurgeGameFoldersCommandHandler> _logger;

    public PurgeGameFoldersCommandHandler(IFileRepo fileRepo, ILogger<PurgeGameFoldersCommandHandler> logger)
    {
        _fileRepo = fileRepo;
        _logger = logger;
    }

    public Task<List<string>> Handle(PurgeGameFoldersCommand request, CancellationToken cancellationToken)
    {
        var deletedFolders = new List<string>();

        var folders = request.GameFolders.Select(_ => 
            Path.Join(request.Game.FullPath, SettingsConsts.GameFolderName, _)
        ).ToList();
        
        foreach (var folder in folders)
        {
            try
            {
                _fileRepo.DeleteDirectory(folder);
                deletedFolders.Add(folder);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error deleting folder {Folder}: {Exception}", folder, ex.Message);
            }
        }

        return Task.FromResult(deletedFolders);
    }
}
