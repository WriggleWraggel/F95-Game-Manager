using GameManager.UI.Features.GameLibrary.Controls;
using GameManager.UI.Features.Settings.Components;

namespace GameManager.UI.Features.GameLibrary.Actions;

public record OpenDownloadFoldersModalAction();

internal class OpenDownloadFoldersActionEffect : Effect<OpenDownloadFoldersModalAction>
{
    public override Task HandleAsync(OpenDownloadFoldersModalAction action, IDispatcher dispatcher)
    {
        try
        {
            dispatcher.Dispatch(new OpenModalAction("Download Folders", typeof(DownloadFoldersComponent)));
        }
        catch ( Exception ex )
        {
            dispatcher.Dispatch(new AddErrorNotificationAction(ex.Message, ex, "Error Opening Download Folders modal"));
        }
        return Task.CompletedTask;
    }
}

