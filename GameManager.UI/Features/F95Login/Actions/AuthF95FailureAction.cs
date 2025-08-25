namespace GameManager.UI.Features.F95Login.Actions;

public class AuthF95FailureAction { }

internal class AuthF95FailureActionReducer : Reducer<F95LoginState, AuthF95FailureAction>
{
    public override F95LoginState Reduce(F95LoginState state, AuthF95FailureAction action) =>
        state with { IsLoggedIn = false };
}




