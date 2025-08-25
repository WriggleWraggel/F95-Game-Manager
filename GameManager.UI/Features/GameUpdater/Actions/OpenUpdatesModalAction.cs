using GameManager.UI.Features.GameUpdater.Components;

namespace GameManager.UI.Features.GameUpdater.Actions;

public record OpenUpdatesModalAction();

internal class OpenUpdatesModalActionEffect : Effect<OpenUpdatesModalAction>
{
    public override Task HandleAsync(OpenUpdatesModalAction action, IDispatcher dispatcher)
    {
        try
        {
            dispatcher.Dispatch(new OpenModalAction("Update Games", typeof(GameUpdaterComponent)));
        }
        catch ( Exception ex )
        {
            dispatcher.Dispatch(new AddErrorNotificationAction(ex.Message, ex, "Error opening Game Updates Modal"));
        }
        return Task.CompletedTask;
    }
}

