namespace GameManager.UI.Features.GameUpdater.Actions.GetUpdates;

public record GetF95UpdatesForLocalGamesSuccessAction();

internal class GetF95UpdatesForLocalGamesSuccessActionReducer : Reducer<GameUpdaterState, GetF95UpdatesForLocalGamesSuccessAction>
{
    public override GameUpdaterState Reduce(GameUpdaterState state, GetF95UpdatesForLocalGamesSuccessAction action) =>
        state with
        {
            SearchingForNewData = false,
            LastSearched = DateTime.Now,
            GamesCheckedProgress = 0,
            UpdateError = null
        };
}
