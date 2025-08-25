using GameManager.Core.MediatR.ProcessLauncher.Commands;

namespace GameManager.UI.Features.GameLibrary.Actions.RunGame;

public record RunGameAction(LocalGame Game);

internal class RunGameActionEffect : Effect<RunGameAction>
{
    private readonly IMediator _mediator;

    public RunGameActionEffect(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task HandleAsync(RunGameAction action, IDispatcher dispatcher)
    {
        try
        {
            var res = await _mediator.Send(new LaunchGameCommand(action.Game));
            dispatcher.Dispatch(new RunGameSuccessAction(action.Game));
        }
        catch ( Exception ex )
        {
            dispatcher.Dispatch(new AddErrorNotificationAction(ex.Message, ex, $"Error running game {action.Game.Title}"));
        }
    }
}

