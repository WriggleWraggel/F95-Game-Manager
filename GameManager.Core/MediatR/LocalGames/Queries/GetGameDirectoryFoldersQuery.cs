using GameManager.Core.Data.Settings;

namespace GameManager.Core.MediatR.LocalGames.Queries;

public record GetGameDirectoryFoldersQuery(LocalGame Game) : IRequest<List<string>>;

internal class GetGameDirectoryFoldersQueryHandler : IRequestHandler<GetGameDirectoryFoldersQuery, List<string>>
{
    private readonly IFileRepo _fileRepo;

    public GetGameDirectoryFoldersQueryHandler(IFileRepo fileRepo) => _fileRepo = fileRepo;
    
    public Task<List<string>> Handle(GetGameDirectoryFoldersQuery request, CancellationToken cancellationToken)
    {
        var gamePath = Path.Join(request.Game.FullPath, SettingsConsts.GameFolderName);

        var directories = _fileRepo.GetFoldersInPath(gamePath);
        
        //return just the folder names
        directories = directories.Select(Path.GetFileName).ToList();

        return Task.FromResult(directories);
    }
}
