using GameManager.UI.Features.GameArchiveImporter.Actions.Map;

namespace GameManager.UI.Features.GameArchiveImporter.Actions.Scan;

public record ScanAllDownloadFoldersSuccessAction(List<string> Files);

internal class ScanAllDownloadFoldersSuccessActionEffect : Effect<ScanAllDownloadFoldersSuccessAction>
{
    private readonly IMessageService _messageService;

    public ScanAllDownloadFoldersSuccessActionEffect(IMessageService messageService) => _messageService = messageService;

    public override Task HandleAsync(ScanAllDownloadFoldersSuccessAction action, IDispatcher dispatcher)
    {
        dispatcher.Dispatch(new MapLocalGamesToDetectedFilesAction());
        return Task.CompletedTask;
    }
}

internal class ScanAllDownloadFoldersSuccessActionReducer : Reducer<GameArchiveImporterState, ScanAllDownloadFoldersSuccessAction>
{
    public override GameArchiveImporterState Reduce(GameArchiveImporterState state, ScanAllDownloadFoldersSuccessAction action)
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
