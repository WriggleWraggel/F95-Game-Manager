using GameManager.Core.MediatR.F95.Commands;
using GameManager.UI.Features.Settings;

namespace GameManager.UI.Features.F95Login.Actions;

public record F95AuthAction();

internal class F95AuthActionEffect : Effect<F95AuthAction>
{
    private readonly IMediator _mediator;
    private readonly IState<SettingsState> _settings;

    public F95AuthActionEffect(IMediator mediator, IState<SettingsState> settings)
    {
        _mediator = mediator;
        _settings = settings;
    }

    public override async Task HandleAsync(F95AuthAction action, IDispatcher dispatcher)
    {
        var username = _settings.Value.Settings.AuthSettings.Username;
        var password = _settings.Value.Settings.AuthSettings.Password;
        if ( string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) )
        {
            dispatcher.Dispatch(new AuthF95FailureAction());
            return;
        }

        var res = await _mediator.Send(new AuthF95Command
        {
            Username = username,
            Password = password,
            Cookies = _settings.Value.Settings.AuthSettings.Cookies
        });

        _settings.Value.Settings.AuthSettings.Cookies = res.Cookies;

        if ( res.Success )
            dispatcher.Dispatch(new AuthF95SuccessAction(res));
        else
            dispatcher.Dispatch(new AuthF95FailureAction());
    }
}
