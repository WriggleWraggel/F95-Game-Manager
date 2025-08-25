using GameManager.Core.Data.Settings;

namespace GameManager.Core.MediatR.LocalGames.Commands;

public record DeleteGameFolderCommand(LocalGame ActionGame) : IRequest;

internal class DeleteGameFolderCommandHandler : IRequestHandler<DeleteGameFolderCommand>
{
    private readonly IFileRepo _fileRepo;
    private readonly ILogger<DeleteGameFolderCommandHandler> _logger;

    public DeleteGameFolderCommandHandler(IFileRepo fileRepo, ILogger<DeleteGameFolderCommandHandler> logger)
    {
        _fileRepo = fileRepo;
        _logger = logger;
    }

    public Task Handle(DeleteGameFolderCommand request, CancellationToken cancellationToken)
    {
        var gameFolder = Path.Join(request.ActionGame.FullPath, SettingsConsts.GameFolderName);

        try
        {
            _fileRepo.DeleteDirectory(gameFolder);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error deleting game folder {GameFolder}: {Exception}", gameFolder, ex.Message);
        }

        return Task.CompletedTask;
    }
}