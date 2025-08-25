using GameManager.Core.MediatR.LocalGames.Commands;
using GameManager.UI.Features.GameArchiveImporter.Actions.UnZipLinkedArchive;

namespace GameManager.UI.Features.GameArchiveImporter.Actions.PurgeGameFolder;

public record PurgeGameFolderThenUnzipAction(LocalGame Game, List<string> GameFolders);

internal class PurgeGameFolderThenUnzipActionEffect : Effect<PurgeGameFolderThenUnzipAction>
{
    private readonly IMediator _mediator;

    public PurgeGameFolderThenUnzipActionEffect(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task HandleAsync(PurgeGameFolderThenUnzipAction action, IDispatcher dispatcher)
    {
        try
        {
             var deletedFolders = await _mediator.Send(new PurgeGameFoldersCommand(action.Game, action.GameFolders));
            
            dispatcher.Dispatch(new AddSuccessNotificationAction(
                $"{string.Join(Environment.NewLine, deletedFolders)}", "Deleted folders")
            );
            
            dispatcher.Dispatch(new UnZipGameAction(action.Game));
        }
        catch ( Exception ex )
        {
            dispatcher.Dispatch(new AddErrorNotificationAction(ex.Message, ex, $"Error purging {action.Game}"));
        }
    }
}