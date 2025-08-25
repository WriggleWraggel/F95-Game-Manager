using GameManager.Core.Data.Settings;

using SevenZip;

namespace GameManager.Core.MediatR.LocalGames.Commands;

/// <summary>
/// Returns the last created directory after unzipping the <paramref name="Game"/> ArchiveFile
/// </summary>
/// <param name="Game"></param>
/// <param name="SevenZipPath"></param>
/// <param name="Progress"></param>
public record UnZipLastDownloadFileCommand(LocalGame Game, string SevenZipPath, IProgress<double>? Progress = null) 
    : IRequest<string>;

internal class UnZipLastDownloadFileCommandFileCommandHandler : IRequestHandler<UnZipLastDownloadFileCommand, string>
{
    public async Task<string> Handle(UnZipLastDownloadFileCommand request, CancellationToken cancellationToken)
    {
        var libraryPath = Path.Combine(request.SevenZipPath, "7z.dll");
        SevenZipBase.SetLibraryPath(libraryPath);

        var archivePath = Path.Join(request.Game.FullPath, SettingsConsts.ArchivesFolderName, request.Game.ArchiveFile);
        var gameFolderPath = Path.Join(request.Game.FullPath, SettingsConsts.GameFolderName);

        using ( var extractor = new SevenZipExtractor(archivePath) )
        {
            //extractor.Extracting += (sender, args) => ()
            //extractor.FileExtractionStarted += (sender, args) => {
            //    statusDescription = String.Format("Extracting file {0}", args.FileInfo.FileName);
            //    Write(statusDescription);
            //};
            await extractor.ExtractArchiveAsync(gameFolderPath);
        }

        var lastCreatedDirectory = Directory
            .GetDirectories(request.Game.FullPath)
            .OrderByDescending(_ => Directory.GetCreationTime(_))
            .FirstOrDefault();

        return lastCreatedDirectory ?? "";
    }
}