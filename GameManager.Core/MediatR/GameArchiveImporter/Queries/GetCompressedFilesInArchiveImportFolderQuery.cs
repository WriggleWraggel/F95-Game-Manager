using GameManager.Core.Data.Settings;

namespace GameManager.Core.MediatR.GameArchiveImporter.Queries;


public record GetCompressedFilesInArchiveImportFolderQuery(ArchiveImportFolder DownloadFolder) : IRequest<List<string>>;

internal class GetCompressedFilesInArchiveImportFolderQueryHandler : IRequestHandler<GetCompressedFilesInArchiveImportFolderQuery, List<string>>
{
    public async Task<List<string>> Handle(GetCompressedFilesInArchiveImportFolderQuery request, CancellationToken cancellationToken)
    {
        var files = Directory
            .EnumerateFiles(request.DownloadFolder.Path, "*.*", new EnumerationOptions { RecurseSubdirectories = true });

        var compressedFiles = files
            .Where(_ => SettingsConsts.CompressedFilesTypeExtensions
                .Any(x => _.EndsWith(x, StringComparison.OrdinalIgnoreCase)))
            .ToList();

        return await Task.FromResult(compressedFiles);
    }
}
