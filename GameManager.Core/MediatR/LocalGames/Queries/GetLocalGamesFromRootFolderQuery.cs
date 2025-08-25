using GameManager.Core.Data.Settings;

namespace GameManager.Core.MediatR.LocalGames.Queries;

public class GetLocalGamesFromRootFolderQuery : IRequest<List<LocalGame>>
{
    public List<GameLibraryFolder>? RootFolders { get; init; }
}

internal class GetLocalGamesFromRootFolderQueryHandler : IRequestHandler<GetLocalGamesFromRootFolderQuery, List<LocalGame>>
{
    public readonly IMediator _mediator;

    public GetLocalGamesFromRootFolderQueryHandler(IMediator mediator) => _mediator = mediator;

    public async Task<List<LocalGame>> Handle(GetLocalGamesFromRootFolderQuery request, CancellationToken cancellationToken)
    {
        if ( request.RootFolders == null )
            throw new ArgumentNullException(nameof(request.RootFolders));

        var files = new List<string>();
        foreach ( var folder in request.RootFolders.Select(_ => _.Path) )
            files.AddRange(Directory.GetFiles(folder, SettingsConsts.GameDataFileName, SearchOption.AllDirectories));

        var loadTasks = files
            .Where(_ => _ != null)
            .Select(_ => _mediator.Send(new GetLocalGameQuery { GameFolderPath = Path.GetDirectoryName(_) ?? throw new Exception("Unable to get directory name") }));

        var res = await Task.WhenAll(loadTasks);

        return res.ToList();
    }
}
