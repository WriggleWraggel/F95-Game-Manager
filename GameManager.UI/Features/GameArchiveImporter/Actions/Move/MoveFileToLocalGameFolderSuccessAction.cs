
using GameManager.UI.Features.GameArchiveImporter.Actions.UnZipLinkedArchive;

namespace GameManager.UI.Features.GameArchiveImporter.Actions.Move;

public record MoveFileToLocalGameFolderSuccessAction(FileMap FileMap);

internal class MoveFileToLocalGameFolderSuccessActionEffect : Effect<MoveFileToLocalGameFolderSuccessAction>
{
    private readonly IMediator _mediator;
    private readonly IState<GameLibraryState> _gameLibraryState;

    public MoveFileToLocalGameFolderSuccessActionEffect(IMediator mediator, IState<GameLibraryState> GameLibraryState)
    {
        _mediator = mediator;
        _gameLibraryState = GameLibraryState;
    }

    public override Task HandleAsync(MoveFileToLocalGameFolderSuccessAction action, IDispatcher dispatcher)
    {
        var game = _gameLibraryState.Value.Games.Single(_ => _.Id == action.FileMap.GameId);
        //TODO Fix path needing \\ prepended
        game.ArchiveFile = "\\" + Path.GetFileName(action.FileMap.FilePath);
        if ( !string.IsNullOrWhiteSpace(action.FileMap.Version) )
            game.Version = action.FileMap.Version;

        //TODO dispatch notification and open the edit modal to set the version if we cant find one
        game.ArchiveFileLastUpdated = DateTime.Now;
        game.ArchiveUnzipedDate = null;
        game.PlayStatus = PlayStatus.NotPlayed;
        game.UpdateAvailable = false;
        dispatcher.Dispatch(new SaveLocalGameAction(game, true));
        return Task.CompletedTask;
    }
}

internal class MoveFileToLocalGameFolderSuccessActionReducer : Reducer<GameArchiveImporterState, MoveFileToLocalGameFolderSuccessAction>
{
    public override GameArchiveImporterState Reduce(GameArchiveImporterState state, MoveFileToLocalGameFolderSuccessAction action)
    {
        var mapToRemove = state.FileMaps.First(_ => _.GameId == action.FileMap.GameId);
        state.FileMaps.Remove(mapToRemove);
        return state with
        {
            FileMaps = state.FileMaps
        };
    }
}
