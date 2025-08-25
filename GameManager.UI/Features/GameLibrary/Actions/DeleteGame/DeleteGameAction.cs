using GameManager.Core.MediatR.LocalGames.Commands;

namespace GameManager.UI.Features.GameLibrary.Actions.DeleteGame;

public record DeleteGameAction(LocalGame Game, DeleteGameOptions Options);

public class DeleteGameActionEffect : Effect<DeleteGameAction>
{
    private readonly IMediator _mediator;

    public DeleteGameActionEffect(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task HandleAsync(DeleteGameAction action, IDispatcher dispatcher)
    {
        try
        {
            if ( action.Options.DeleteGame )
            {
                await _mediator.Send(new DeleteLocalGameCommand(action.Game));
                dispatcher.Dispatch(new DeleteGameSuccessAction(action.Game, action.Options));
                return;
            }

            if ( action.Options.DeleteArchives )
            {
                await _mediator.Send(new DeleteArchiveFolderCommand(action.Game));
            }

            if ( action.Options.DeleteUnarchivedGame )
            {
                await _mediator.Send(new DeleteGameFolderCommand(action.Game));
            }

            if ( action.Options.DeleteMods )
            {
                await _mediator.Send(new DeleteModsFolderCommand(action.Game));
            }
            
            dispatcher.Dispatch(new DeleteGameSuccessAction(action.Game, action.Options));
        }
        catch ( Exception ex )
        {
            dispatcher.Dispatch(new AddErrorNotificationAction(ex.Message, ex, 
                $"Error deleting {action.Game.Title}")
            );
        }
    }
}

public class DeleteGameActionReducer : Reducer<GameLibraryState, DeleteGameAction>
{
    public override GameLibraryState Reduce(GameLibraryState state, DeleteGameAction action)
    {
        var currentGameMetaData = state.GameMetaData.FirstOrDefault(_ => _.Id == action.Game.Id);

        if ( currentGameMetaData == null )
        {
            currentGameMetaData = new GameMetaData
            {
                Id = action.Game.Id,
            };
            state.GameMetaData.Add(currentGameMetaData);
        }

        currentGameMetaData.Processing = true;
        return state with
        {
            GameMetaData = state.GameMetaData
        };
    }
}