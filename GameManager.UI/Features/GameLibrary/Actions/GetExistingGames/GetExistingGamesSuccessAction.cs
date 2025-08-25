using GameManager.UI.Features.GameLibrary;

namespace GameManager.UI.Features.GameLibrary.Actions.GetExistingGames;

public record GetExistingGamesSuccessAction(List<LocalGame> ManagedGames);

internal class GetExistingGamesSuccessActionReducer : Reducer<GameLibraryState, GetExistingGamesSuccessAction>
{
    public override GameLibraryState Reduce(GameLibraryState state, GetExistingGamesSuccessAction action)
    {
        var gamesList = state.Games;
        gamesList.RemoveAll(_ => _.Saved == true);
        gamesList.AddRange(action.ManagedGames);
        return state with
        {
            Scanning = false,
            ManagedGamesLoaded = true,
            Games = gamesList
        };
    }
}