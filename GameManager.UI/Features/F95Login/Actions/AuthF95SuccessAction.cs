using GameManager.Core.MediatR.F95.Commands;
using GameManager.UI.Features.GameUpdater.Actions.GetUpdates;
using GameManager.UI.Features.Settings.Actions.Save;

namespace GameManager.UI.Features.F95Login.Actions;

public record AuthF95SuccessAction(AuthResult AuthResult);

internal class AuthF95SuccessActionEffect : Effect<AuthF95SuccessAction>
{
    public override Task HandleAsync(AuthF95SuccessAction action, IDispatcher dispatcher)
    {
        dispatcher.Dispatch(new SaveSettingsAction());
        return Task.CompletedTask;
    }
}

internal class AuthF95SuccessActionReducer : Reducer<F95LoginState, AuthF95SuccessAction>
{
    public override F95LoginState Reduce(F95LoginState state, AuthF95SuccessAction action) =>
        state with { IsLoggedIn = true };
}
