using GameManager.Core.Data.Settings;

namespace GameManager.Core.MediatR.LocalGames.Commands;

public record DeleteModsFolderCommand(LocalGame ActionGame) : IRequest;

internal class DeleteModsFolderCommandHandler : IRequestHandler<DeleteModsFolderCommand>
{
    private readonly IFileRepo _fileRepo;
    private readonly ILogger<DeleteModsFolderCommandHandler> _logger;

    public DeleteModsFolderCommandHandler(IFileRepo fileRepo, ILogger<DeleteModsFolderCommandHandler> logger)
    {
        _fileRepo = fileRepo;
        _logger = logger;
    }

    public Task Handle(DeleteModsFolderCommand request, CancellationToken cancellationToken)
    {
        var modsFolder = Path.Join(request.ActionGame.FullPath, SettingsConsts.ModsFolderName);

        try
        {
            _fileRepo.EmptyDirectory(modsFolder);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error deleting mods folder {ModsFolder}: {Exception}", modsFolder, ex.Message);
        }

        return Task.CompletedTask;
    }
}