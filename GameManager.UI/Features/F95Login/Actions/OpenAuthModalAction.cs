using GameManager.UI.Features.Settings.Components;

namespace GameManager.UI.Features.F95Login.Actions;

public record OpenAuthModalAction();

internal class OpenAuthModalActionEffect : Effect<OpenAuthModalAction>
{
    public override Task HandleAsync(OpenAuthModalAction action, IDispatcher dispatcher)
    {
        try
        {
            dispatcher.Dispatch(new OpenModalAction("Auth Settings", typeof(F95AuthDetailsComponent)));
        }
        catch ( Exception ex )
        {
            dispatcher.Dispatch(new AddErrorNotificationAction(ex.Message, ex, "Error Opening Auth Settings modal"));
        }
        return Task.CompletedTask;
    }
}

