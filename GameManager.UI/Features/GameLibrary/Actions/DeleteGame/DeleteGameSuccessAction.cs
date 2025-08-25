namespace GameManager.UI.Features.GameLibrary.Actions.DeleteGame;

public record DeleteGameSuccessAction(LocalGame Game, DeleteGameOptions Options);

public class DeleteGameSuccessActionEffect : Effect<DeleteGameSuccessAction>
{
    private readonly IMediator _mediator;

    public DeleteGameSuccessActionEffect(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override Task HandleAsync(DeleteGameSuccessAction action, IDispatcher dispatcher)
    {
        if ( action.Options.DeleteGame )
        {
            dispatcher.Dispatch(new AddSuccessNotificationAction($"{action.Game.Title}", "Game Deleted"));
        }
        else
        {
            
            if ( action.Options.DeleteArchives )
            {
                action.Game.ArchiveFile = "";
            }

            if ( action.Options.DeleteUnarchivedGame )
            {
                action.Game.LaunchExePath = "";
            }

            var message = (action.Options.DeleteArchives ? "Archives, " : "") +
                          (action.Options.DeleteUnarchivedGame ? "Unarchived Game, " : "") +
                          (action.Options.DeleteMods ? "Mods, " : "");
            
            dispatcher.Dispatch(new AddSuccessNotificationAction(
                $"{action.Game.Title}", $"{message} Folder/s Deleted")
            );
            dispatcher.Dispatch(new SaveLocalGameAction(action.Game));
        }

        dispatcher.Dispatch(new CloseModalAction());
        return Task.CompletedTask;
    }
}

public class DeleteGameSuccessActionReducer : Reducer<GameLibraryState, DeleteGameSuccessAction>
{
    public override GameLibraryState Reduce(GameLibraryState state, DeleteGameSuccessAction action)
    {
        var games = state.Games;
        var gamesMetaData = state.GameMetaData;
        var currentGame = games.FirstOrDefault(_ => _.Id == action.Game.Id);
        var currentGameMetaData = gamesMetaData.FirstOrDefault(_ => _.Id == action.Game.Id);
        if(currentGame == null || currentGameMetaData == null)
        {
            return state;
        }
        
        if ( action.Options.DeleteGame )
        {
            gamesMetaData.Remove(currentGameMetaData);
            games.Remove(currentGame);
        }
        else
        {
            currentGameMetaData.Processing = false;
        }
        
        return state with
        {
            Games = games,
            GameMetaData = gamesMetaData
        };
    }
}