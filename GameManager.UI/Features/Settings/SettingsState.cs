namespace GameManager.UI.Features.Settings;

[FeatureState]
public record SettingsState
{
    public AppSettings Settings { get; init; } = new();
    public bool Initialized { get; init; } = false;
    public bool Busy { get; init; } = false;
}
