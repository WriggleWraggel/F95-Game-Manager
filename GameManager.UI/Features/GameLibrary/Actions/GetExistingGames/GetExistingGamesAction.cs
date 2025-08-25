using GameManager.Core.MediatR.LocalGames.Queries;

namespace GameManager.UI.Features.GameLibrary.Actions.GetExistingGames;

public record GetExistingGamesAction(List<GameLibraryFolder> RootFolders);

internal class GetExistingGamesActionEffect : Effect<GetExistingGamesAction>
{
    private readonly IMediator _mediator;

    public GetExistingGamesActionEffect(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task HandleAsync(GetExistingGamesAction action, IDispatcher dispatcher)
    {
        try
        {
            var res = await _mediator.Send(new GetLocalGamesFromRootFolderQuery { RootFolders = action.RootFolders });
            dispatcher.Dispatch(new GetExistingGamesSuccessAction(res));
        }
        catch ( Exception ex )
        {
            dispatcher.Dispatch(new AddErrorNotificationAction(ex.Message, ex, "Error getting Games Libraries"));
        }
    }
}

internal class GetExistingGamesActionReducer : Reducer<GameLibraryState, GetExistingGamesAction>
{
    public override GameLibraryState Reduce(GameLibraryState state, GetExistingGamesAction action) =>
        state with
        {
            Scanning = true,
        };
}

