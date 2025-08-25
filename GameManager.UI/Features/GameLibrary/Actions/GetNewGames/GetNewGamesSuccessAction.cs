namespace GameManager.UI.Features.GameLibrary.Actions.GetNewGames;

internal record GetNewGamesSuccessAction(List<LocalGame> UnmanagedGames);

internal class GetNewGamesSuccessActionReducer : Reducer<GameLibraryState, GetNewGamesSuccessAction>
{
    public override GameLibraryState Reduce(GameLibraryState state, GetNewGamesSuccessAction action)
    {
        var gamesList = state.Games;
        gamesList.RemoveAll(_ => _.Saved == false);
        gamesList.AddRange(action.UnmanagedGames);
        return state with
        {
            Scanning = false,
            Games = gamesList
        };
    }
}