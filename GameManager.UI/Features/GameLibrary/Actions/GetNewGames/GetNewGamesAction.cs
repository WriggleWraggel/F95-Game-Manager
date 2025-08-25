using GameManager.Core.MediatR.LocalGames.Queries;

namespace GameManager.UI.Features.GameLibrary.Actions.GetNewGames;

public record GetNewGamesAction(GameLibraryFolder RootFolder);

internal class GetNewGamesActionEffect : Effect<GetNewGamesAction>
{
    private readonly IMediator _mediator;

    public GetNewGamesActionEffect(IMediator mediator) => _mediator = mediator;

    public override async Task HandleAsync(GetNewGamesAction action, IDispatcher dispatcher)
    {
        try
        {
            var res = await _mediator.Send(new ScanRootFolderQuery
            {
                RootFolder = action.RootFolder,
            });

            dispatcher.Dispatch(new GetNewGamesSuccessAction(res));

        }
        catch ( Exception ex )
        {
            dispatcher.Dispatch(new AddErrorNotificationAction("Error while scanning for new games", ex, "Scanning Failure"));
            dispatcher.Dispatch(new GetNewGamesFailureAction());
        }
    }
}

internal class GetNewGamesActionReducer : Reducer<GameLibraryState, GetNewGamesAction>
{
    public override GameLibraryState Reduce(GameLibraryState state, GetNewGamesAction action) =>
        state with { Scanning = true };
}
