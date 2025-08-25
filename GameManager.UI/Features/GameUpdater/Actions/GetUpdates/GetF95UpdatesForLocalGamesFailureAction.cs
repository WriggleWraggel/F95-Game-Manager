namespace GameManager.UI.Features.GameUpdater.Actions.GetUpdates;

public record GetF95UpdatesForLocalGamesFailureAction(Exception Exception);

internal class GetF95UpdatesForLocalGamesFailureActionEffect : Effect<GetF95UpdatesForLocalGamesFailureAction>
{
    public override Task HandleAsync(GetF95UpdatesForLocalGamesFailureAction action, IDispatcher dispatcher)
    {
        dispatcher.Dispatch(new AddErrorNotificationAction(action.Exception.Message, action.Exception, "Error checking for game updates"));
        return Task.CompletedTask;
    }
}

internal class GetF95UpdatesForLocalGamesFailureActionReducer : Reducer<GameUpdaterState, GetF95UpdatesForLocalGamesFailureAction>
{
    public override GameUpdaterState Reduce(GameUpdaterState state, GetF95UpdatesForLocalGamesFailureAction action) =>
        state with
        {
            SearchingForNewData = false,
            GamesCheckedProgress = 0,
            UpdateError = action.Exception
        };
}
