using GameManager.UI.Features.GameLibrary.Actions.EditGame;
using GameManager.UI.Features.GameLibrary.Actions.OpenGameFolder;

namespace GameManager.UI.Features.GameArchiveImporter.Actions.UnZipLinkedArchive;

public record UnZipGameSuccessAction(LocalGame Game, string ExtractedPath);

internal class UnZipGameSuccessActionEffect : Effect<UnZipGameSuccessAction>
{
    private readonly IMessageService _messageService;

    public UnZipGameSuccessActionEffect(IMessageService messageService)
    {
        _messageService = messageService;
    }

    public override Task HandleAsync(UnZipGameSuccessAction action, IDispatcher dispatcher)
    {
        dispatcher.Dispatch(new OpenGameFolderAction(action.Game));
        dispatcher.Dispatch(new OpenEditModalAction(action.Game));

        return Task.CompletedTask;
    }
}

internal class UnZipGameSuccessActionReducer : Reducer<GameLibraryState, UnZipGameSuccessAction>
{
    public override GameLibraryState Reduce(GameLibraryState state, UnZipGameSuccessAction action)
    {
        var gameMetaData = state.GameMetaData.FirstOrDefault(_ => _.Id == action.Game.Id);

        if ( gameMetaData == null )
        {
            gameMetaData = new GameMetaData
            {
                Id = action.Game.Id,
            };
            state.GameMetaData.Add(gameMetaData);
        }

        var game = state.Games.First(_ => _.Id == action.Game.Id);
        game.ArchiveUnzipedDate = DateTime.Now;

        gameMetaData.Processing = false;
        return state with
        {
            Games = state.Games,
            GameMetaData = state.GameMetaData
        };
    }
}