namespace GameManager.UI.Features.GameLibrary.Actions.RunGame;

public record RunGameSuccessAction(LocalGame Game);

internal class RunGameSuccessActionEffect : Effect<RunGameSuccessAction>
{
    public override Task HandleAsync(RunGameSuccessAction action, IDispatcher dispatcher)
    {
        dispatcher.Dispatch(new SaveLocalGameAction(action.Game));
        return Task.CompletedTask;
    }
}

internal class RunGameSuccessActionReducer : Reducer<GameLibraryState, RunGameSuccessAction>
{
    public override GameLibraryState Reduce(GameLibraryState state, RunGameSuccessAction action)
    {
        var game = state.Games.Single(_ => _.Id == action.Game.Id);
        game.LastPlayed = DateTime.Now;
        game.PlayStatus = PlayStatus.Playing;
        game.TiemsPlayed++;

        return state with
        {
            Games = state.Games,
        };
    }
}

