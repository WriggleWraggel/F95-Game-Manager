using GameManager.Core.Data.Settings;

namespace GameManager.Core.MediatR.LocalGames.Queries;

public class FolderData
{
    public List<string> PossibleLastDownloadFiles { get; set; } = new List<string>();
    public List<string> PossibleGameFiles { get; set; } = new List<string>();
}

public record GetPossibleGameFilesFromDirectoryQuery(LocalGame Game) : IRequest<FolderData>;

internal class GetPossibleGameFilesFromDirectoryQueryHandler : IRequestHandler<GetPossibleGameFilesFromDirectoryQuery, FolderData>
{
    private readonly IFileRepo _fileRepo;

    public GetPossibleGameFilesFromDirectoryQueryHandler(IFileRepo fileRepo) => _fileRepo = fileRepo;

    public Task<FolderData> Handle(GetPossibleGameFilesFromDirectoryQuery request, CancellationToken cancellationToken)
    {
        var archivePath = Path.Join(request.Game.FullPath, SettingsConsts.ArchivesFolderName);
        var gamePath = Path.Join(request.Game.FullPath, SettingsConsts.GameFolderName);

        var compressedFiles = _fileRepo.GetCompressedFiles(archivePath);
        var gameFiles = _fileRepo
            .GetGameExecutableFiles(gamePath);

        var folderData = new FolderData
        {
            PossibleLastDownloadFiles = compressedFiles,
            PossibleGameFiles = gameFiles,
        };

        return Task.FromResult(folderData);
    }
}
