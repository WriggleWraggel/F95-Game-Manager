using GameManager.Core.MediatR.GameArchiveImporter.Queries;

namespace GameManager.UI.Features.GameArchiveImporter.Actions.Scan;

public record ScanDownloadFolderAction(ArchiveImportFolder DownloadFolder);

internal class ScanDownloadFolderActionEffect : Effect<ScanDownloadFolderAction>
{
    private readonly IMediator _mediator;

    public ScanDownloadFolderActionEffect(IMediator mediator) => _mediator = mediator;

    public override async Task HandleAsync(ScanDownloadFolderAction action, IDispatcher dispatcher)
    {
        try
        {
            var possibleCompressedGames = await _mediator.Send(new GetCompressedFilesInArchiveImportFolderQuery(action.DownloadFolder));

            dispatcher.Dispatch(new ScanDownloadFolderSuccessAction(possibleCompressedGames));
        }
        catch ( Exception ex )
        {
            dispatcher.Dispatch(new AddErrorNotificationAction(ex.Message, ex, "Error scanning downloads folder"));
        }
    }
}

internal class ScanDownloadFolderActionReducer : Reducer<GameArchiveImporterState, ScanDownloadFolderAction>
{
    public override GameArchiveImporterState Reduce(GameArchiveImporterState state, ScanDownloadFolderAction action) =>
        state with
        {
            FileMaps = new(),
            Scanning = true
        };
}
