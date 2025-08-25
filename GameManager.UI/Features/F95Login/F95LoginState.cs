
namespace GameManager.UI.Features.F95Login;

[FeatureState]
public record F95LoginState
{
    public bool IsLoggedIn { get; init; } = false;
}
