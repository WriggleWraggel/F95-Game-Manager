using GameManager.Core.Data.Settings;

namespace GameManager.Core.MediatR.LocalGames.Commands;

public record DeleteArchiveFolderCommand(LocalGame ActionGame) : IRequest;

internal class DeleteArchiveFolderCommandHandler : IRequestHandler<DeleteArchiveFolderCommand>
{
    private readonly IFileRepo _fileRepo;
    private readonly ILogger<DeleteArchiveFolderCommandHandler> _logger;

    public DeleteArchiveFolderCommandHandler(IFileRepo fileRepo, ILogger<DeleteArchiveFolderCommandHandler> logger)
    {
        _fileRepo = fileRepo;
        _logger = logger;
    }

    public Task Handle(DeleteArchiveFolderCommand request, CancellationToken cancellationToken)
    {
        var archiveFolder = Path.Join(request.ActionGame.FullPath, SettingsConsts.ArchivesFolderName);

        try
        {
            _fileRepo.EmptyDirectory(archiveFolder);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error deleting archive folder {ArchiveFolder}: {Exception}", archiveFolder, ex.Message);
        }

        return Task.CompletedTask;
    }
}