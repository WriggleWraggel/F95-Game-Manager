using GameManager.Core.MediatR.LocalGames.Commands;

namespace GameManager.UI.Features.GameLibrary.Actions.SaveGame;

public record SaveLocalGameAction(LocalGame LocalGame, bool ShowSuccessSaveMessage = false);

internal class SaveLocalGameActionEffect : Effect<SaveLocalGameAction>
{
    private readonly IMediator _mediator;
    public SaveLocalGameActionEffect(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task HandleAsync(SaveLocalGameAction action, IDispatcher dispatcher)
    {
        try
        {
            var game = await _mediator.Send(new SaveLocalGameCommand(action.LocalGame));
            dispatcher.Dispatch(new SaveLocalGameSuccessAction(game, action.ShowSuccessSaveMessage));
        }
        catch ( Exception ex )
        {
            dispatcher.Dispatch(new AddErrorNotificationAction(ex.Message, ex, $"Error saving local game {action.LocalGame.Title}"));
        }
    }
}