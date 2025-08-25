using GameManager.Core.Data.Settings;

namespace GameManager.Core.MediatR.LocalGames.Queries;

public class ScanRootFolderQuery : IRequest<List<LocalGame>>
{
    public GameLibraryFolder RootFolder { get; init; } = new();
}

internal class ScanRootFolderQueryHandler : IRequestHandler<ScanRootFolderQuery, List<LocalGame>>
{

    public ScanRootFolderQueryHandler()
    {
    }

    public Task<List<LocalGame>> Handle(ScanRootFolderQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var gameDirectories = Directory.GetDirectories(request.RootFolder.Path);
            //Get the folder paths for each saved game to exclude from the list
            var existingGamesDirectories = Directory.EnumerateFiles(request.RootFolder.Path, SettingsConsts.GameDataFileName,
                new EnumerationOptions
                {
                    MaxRecursionDepth = 1,
                    RecurseSubdirectories = true
                })
                .Select(_ => Path.GetDirectoryName(_))
                .ToList();

            var localGames = gameDirectories
                .Where(_ => !existingGamesDirectories.Contains(_))
                .Select(_ => new LocalGame
                {
                    RootFolder = request.RootFolder,
                    FolderName = Path.GetFileName(_) ?? string.Empty,
                    Title = Path.GetFileName(_) ?? string.Empty,
                    Saved = false,
                }).ToList();

            return Task.FromResult(localGames);
        }
        catch ( Exception )
        {
            throw;
        }
    }
}

