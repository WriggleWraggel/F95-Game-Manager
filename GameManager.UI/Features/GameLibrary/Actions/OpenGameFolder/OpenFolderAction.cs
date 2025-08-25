using System.Text.RegularExpressions;

using GameManager.Core.MediatR.LocalGames.Queries;
using GameManager.Core.MediatR.ProcessLauncher.Commands;

namespace GameManager.UI.Features.GameLibrary.Actions.OpenGameFolder;

public record OpenGameFolderAction(LocalGame Game);

internal class OpenFolderActionEffect : Effect<OpenGameFolderAction>
{
    private readonly IMediator _mediator;

    public OpenFolderActionEffect(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task HandleAsync(OpenGameFolderAction action, IDispatcher dispatcher)
    {
        try
        {
            await _mediator.Send(new OpenFolderCommand(action.Game));
        }
        catch ( Exception ex )
        {
            dispatcher.Dispatch(new AddErrorNotificationAction(ex.Message, ex, $"Error opening folder for {action.Game.Title}"));
        }
    }
}

