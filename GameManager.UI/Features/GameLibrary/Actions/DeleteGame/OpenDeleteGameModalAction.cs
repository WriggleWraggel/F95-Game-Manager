using GameManager.UI.Features.GameLibrary.Components.ExistingGames;

namespace GameManager.UI.Features.GameLibrary.Actions.DeleteGame;

public record OpenDeleteGameModalAction(LocalGame Game);

public class OpenDeleteGameModalActionEffect : Effect<OpenDeleteGameModalAction>
{
    public override Task HandleAsync(OpenDeleteGameModalAction action, IDispatcher dispatcher)
    {
        try
        {
            dispatcher.Dispatch(new OpenModalAction($"Delete {action.Game.Title}", typeof(DeleteGameComponent),
                new Dictionary<string, object>
                {
                    { nameof(DeleteGameComponent.Game), action.Game },
                }));
        }
        catch (Exception ex)
        {
            dispatcher.Dispatch(new AddErrorNotificationAction(
                ex.Message, ex, 
                $"Error opening delete modal for {action.Game.Title}")
            );
        }
        return Task.CompletedTask;
    }
}