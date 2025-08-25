using GameManager.Core.MediatR.ProcessLauncher.Commands;

namespace GameManager.UI.Features.IGames.Actions;

public record OpenGameInBrowserAction(IGame Game);

internal class OpenGameInBrowserActionEffect : Effect<OpenGameInBrowserAction>
{
    private readonly IMediator _mediator;

    public OpenGameInBrowserActionEffect(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task HandleAsync(OpenGameInBrowserAction action, IDispatcher dispatcher)
    {
        try
        {
            var res = await _mediator.Send(new LaunchBrowserCommand(action.Game.GameUrl));
        }
        catch ( Exception ex )
        {
            dispatcher.Dispatch(new AddErrorNotificationAction(ex.Message, ex, $"Error opening {action.Game.Title} in browser"));
        }
    }
}

