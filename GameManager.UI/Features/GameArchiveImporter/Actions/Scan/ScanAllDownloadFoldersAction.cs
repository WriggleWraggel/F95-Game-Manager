using GameManager.Core.MediatR.GameArchiveImporter.Queries;
using GameManager.UI.Features.Settings;

namespace GameManager.UI.Features.GameArchiveImporter.Actions.Scan;

public record ScanAllDownloadFoldersAction();

internal class ScanDownloadFoldersActionEffect : Effect<ScanAllDownloadFoldersAction>
{
    private readonly IMediator _mediator;
    private readonly IState<SettingsState> _settings;

    public ScanDownloadFoldersActionEffect(IMediator mediator, IState<SettingsState> settings)
    {
        _mediator = mediator;
        _settings = settings;
    }

    public override async Task HandleAsync(ScanAllDownloadFoldersAction action, IDispatcher dispatcher)
    {
        try
        {
            var possibleCompressedGames = await _mediator.Send(
                new GetAllCompressedFilesInFoldersQuery(_settings.Value.Settings.ImportFolders));

            dispatcher.Dispatch(new ScanAllDownloadFoldersSuccessAction(possibleCompressedGames));
        }
        catch ( Exception ex )
        {
            dispatcher.Dispatch(new AddErrorNotificationAction(ex.Message, ex, "Error scanning root folders"));
        }
    }
}

internal class ScanDownloadFoldersActionReducer : Reducer<GameArchiveImporterState, ScanAllDownloadFoldersAction>
{
    public override GameArchiveImporterState Reduce(GameArchiveImporterState state, ScanAllDownloadFoldersAction action) =>
        state with
        {
            FileMaps = new(),
            Scanning = true
        };
}
