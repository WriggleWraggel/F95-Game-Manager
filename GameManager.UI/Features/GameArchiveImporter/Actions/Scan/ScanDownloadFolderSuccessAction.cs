using GameManager.UI.Features.GameArchiveImporter.Actions.Map;

namespace GameManager.UI.Features.GameArchiveImporter.Actions.Scan;

public record ScanDownloadFolderSuccessAction(List<string> Files);

internal class ScanDownloadFolderSuccessActionEffect : Effect<ScanDownloadFolderSuccessAction>
{
    public override Task HandleAsync(ScanDownloadFolderSuccessAction action, IDispatcher dispatcher)
    {
        dispatcher.Dispatch(new MapLocalGamesToDetectedFilesAction());
        return Task.CompletedTask;
    }
}

internal class ScanDownloadFolderSuccessActionReducer : Reducer<GameArchiveImporterState, ScanDownloadFolderSuccessAction>
{
    public override GameArchiveImporterState Reduce(GameArchiveImporterState state, ScanDownloadFolderSuccessAction action)
    {
        foreach ( var file in action.Files )
            if ( !state.FileMaps.Any(_ => _.FilePath.Equals(file, StringComparison.OrdinalIgnoreCase)) )
                state.FileMaps.Add(new FileMap
                {
                    FilePath = file,
                });

        return state with
        {
            Scanning = false,
            FileMaps = state.FileMaps
        };
    }
}
