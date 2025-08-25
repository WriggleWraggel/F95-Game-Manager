using GameManager.Core.Data.Settings;

namespace GameManager.Core.MediatR.GameArchiveImporter.Comands;

public record MoveDownloadFileCommand(string filePath, LocalGame Game) : IRequest;

internal class MoveDownloadFileCommandHandler : IRequestHandler<MoveDownloadFileCommand>
{
    private readonly IFileRepo _fileRepo;

    public MoveDownloadFileCommandHandler(IFileRepo fileRepo)
    {
        _fileRepo = fileRepo;
    }

    public Task Handle(MoveDownloadFileCommand request, CancellationToken cancellationToken)
    {
        var dest = Path.Join(
            request.Game.FullPath,
            SettingsConsts.ArchivesFolderName,
            Path.GetFileName(request.filePath));

        _fileRepo.MoveFile(request.filePath, dest, true);

        return Task.CompletedTask;
    }
}
