namespace GameManager.UI.Features.GameLibrary.Actions.SaveGame;
internal record SaveLocalGameSuccessAction(LocalGame LocalGame, bool ShowSuccessSaveMessage);

internal class SaveLocalGameSuccessActionEffect : Effect<SaveLocalGameSuccessAction>
{
    public override Task HandleAsync(SaveLocalGameSuccessAction action, IDispatcher dispatcher)
    {
        if ( action.ShowSuccessSaveMessage )
            dispatcher.Dispatch(new AddSuccessNotificationAction($"{action.LocalGame.Title}", "Game Saved"));

        return Task.CompletedTask;
    }
}

internal class SaveLocalGameSuccessActionReducer : Reducer<GameLibraryState, SaveLocalGameSuccessAction>
{
    public override GameLibraryState Reduce(GameLibraryState state, SaveLocalGameSuccessAction action)
    {
        var games = state.Games;

        var existingGame = games.FirstOrDefault(_ => _.Id == action.LocalGame.Id);

        if ( existingGame == null )
            games.Add(action.LocalGame);
        else
            games[games.IndexOf(existingGame)] = action.LocalGame;

        return state with
        {
            Games = games
        };
    }
}
