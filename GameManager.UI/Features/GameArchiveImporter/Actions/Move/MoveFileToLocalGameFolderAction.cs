
using GameManager.Core.MediatR.GameArchiveImporter.Comands;
using GameManager.Core.MediatR.LocalGames.Commands;

namespace GameManager.UI.Features.GameArchiveImporter.Actions.Move;

public record MoveFileToLocalGameFolderAction(FileMap FileMap);

internal class MoveFileToLocalGameFolderActionEffect : Effect<MoveFileToLocalGameFolderAction>
{
    private readonly IMediator _mediator;
    private readonly IState<GameLibraryState> _gameLibraryState;

    public MoveFileToLocalGameFolderActionEffect(IMediator mediator, IState<GameLibraryState> GameLibraryState)
    {
        _mediator = mediator;
        _gameLibraryState = GameLibraryState;
    }

    public override async Task HandleAsync(MoveFileToLocalGameFolderAction action, IDispatcher dispatcher)
    {
        try
        {
            var game = _gameLibraryState.Value.Games.First(_ => _.Id == action.FileMap.GameId);

            //delete the old version if the we need to
            if ( !string.IsNullOrWhiteSpace(game.ArchiveFile) && action.FileMap.RemoveOldFile )
            {
                try
                {
                    await _mediator.Send(new DeleteOldLastDownloadFileCommandFileCommand(game));
                    dispatcher.Dispatch(new AddSuccessNotificationAction(
                        $"Deleted {game.ArchiveFile}", "Delete old archive")
                    );
                }
                catch ( Exception e )
                {
                    dispatcher.Dispatch(new AddErrorNotificationAction(
                        $"Error deleting {game.ArchiveFile}", e, "File Deletion Error")
                    );
                }
            }

            await _mediator.Send(new MoveDownloadFileCommand(action.FileMap.FilePath, game));
            dispatcher.Dispatch(new MoveFileToLocalGameFolderSuccessAction(action.FileMap));
        }
        catch ( Exception ex )
        {
            dispatcher.Dispatch(new AddErrorNotificationAction(ex.Message, ex, $"Error moving {action.FileMap.FilePath}"));
        }
    }
}

internal class MoveFileToLocalGameFolderActionReducer : Reducer<GameArchiveImporterState, MoveFileToLocalGameFolderAction>
{
    public override GameArchiveImporterState Reduce(GameArchiveImporterState state, MoveFileToLocalGameFolderAction action)
    {
        state.FileMaps.First(_ => _.GameId == action.FileMap.GameId).Processing = true;
        return state;
    }
}
