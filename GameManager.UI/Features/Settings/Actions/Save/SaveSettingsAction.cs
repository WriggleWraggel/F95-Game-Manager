using GameManager.Core.MediatR.Settings.Commands;
using GameManager.UI.Features.Settings;

namespace GameManager.UI.Features.Settings.Actions.Save;

public record SaveSettingsAction();


internal class SaveSettingsActionEffect : Effect<SaveSettingsAction>
{
    private readonly IMediator _mediator;
    private readonly IState<SettingsState> _settingsState;

    public SaveSettingsActionEffect(IMediator mediator, IState<SettingsState> settingsState)
    {
        _mediator = mediator;
        _settingsState = settingsState;
    }

    public override async Task HandleAsync(SaveSettingsAction action, IDispatcher dispatcher)
    {
        try
        {
            await _mediator.Send(new SaveSettingsCommand
            {
                AppSettings = _settingsState.Value.Settings
            });

            dispatcher.Dispatch(new SaveSettingsSuccessAction());
        }
        catch ( Exception ex )
        {
            dispatcher.Dispatch(new AddErrorNotificationAction(ex.Message, ex, "Error saving settings"));
        }
    }
}

internal class SaveSettingsActionReduce : Reducer<SettingsState, SaveSettingsAction>
{
    public override SettingsState Reduce(SettingsState state, SaveSettingsAction action) =>
        state with
        {
            Busy = true
        };
}
