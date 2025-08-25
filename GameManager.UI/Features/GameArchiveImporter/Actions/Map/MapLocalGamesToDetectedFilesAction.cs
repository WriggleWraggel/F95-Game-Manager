using System.Text.RegularExpressions;

using GameManager.Core.MediatR.GameArchiveImporter.Queries;

using Quickenshtein;

namespace GameManager.UI.Features.GameArchiveImporter.Actions.Map;

public record MapLocalGamesToDetectedFilesAction();

internal class MapLocalGamesToCompressedFilesActionEffect : Effect<MapLocalGamesToDetectedFilesAction>
{
    private readonly IMediator _mediator;
    private readonly IState<GameLibraryState> _gameLibState;
    private readonly IState<GameArchiveImporterState> _gameArchiveImporterState;

    public MapLocalGamesToCompressedFilesActionEffect(IMediator mediator, IState<GameLibraryState> localGameState, IState<GameArchiveImporterState> GameArchiveImporterState)
    {
        _mediator = mediator;
        _gameLibState = localGameState;
        _gameArchiveImporterState = GameArchiveImporterState;
    }

    public override async Task HandleAsync(MapLocalGamesToDetectedFilesAction action, IDispatcher dispatcher)
    {
        try
        {
            var mappedGames = new List<FileMap>();
            var filePaths = _gameArchiveImporterState.Value.FileMaps
                .Where(_ => _.GameId == null)
                .Select(_ => _.FilePath);

            
            var tasks = filePaths.Select(async filePath =>
            {
                var likelyMatch = await _mediator.Send(
                    new GetMatchingGameForArchiveQuery(filePath, _gameLibState.Value.Games)
                );

                if (likelyMatch != null)
                {
                    return new FileMap
                    {
                        FilePath = filePath,
                        Version = likelyMatch.F95Game?.Version ?? "",
                        GameId = likelyMatch.Id
                    };
                }
                return null;
            }).ToList();

            var results = await Task.WhenAll(tasks);

            mappedGames.AddRange(results.Where(result => result != null)!);

            dispatcher.Dispatch(new MapLocalGamesToDetectedFilesSuccessAction(mappedGames));
        }
        catch ( Exception ex )
        {
            dispatcher.Dispatch(new AddErrorNotificationAction(ex.Message, ex, "Error while mapping game to compressed file"));
        }

        return;
    }

    private static string SanitizeString(string title) =>
        Regex.Replace(title, @"[^a-zA-Z]*", "")
            .Trim()
            .ToLower();
}