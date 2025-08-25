namespace GameManager.UI.Features.GameUpdater;

[FeatureState]
public record GameUpdaterState
{
    public bool SearchingForNewData { get; init; }
    public DateTime? LastSearched { get; init; } = null;

    public int GamesCheckedProgress { get; init; }
    public Exception? UpdateError { get; init; } = null;
}
