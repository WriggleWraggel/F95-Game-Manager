using System.Reflection;

namespace GameManager.UI.Features.GameLibrary;


[FeatureState]
public record GameLibraryState
{
    public List<LocalGame> Games { get; init; } = new List<LocalGame>();
    public List<GameMetaData> GameMetaData { get; init; } = new List<GameMetaData>();
    public GameLibaryFilter Filter { get; init; } = new GameLibaryFilter();
    public bool ManagedGamesLoaded { get; set; } = false;
    public bool Scanning { get; init; } = false;
}


public class GameMetaData
{
    public Guid Id { get; set; }
    public bool Processing { get; set; }
}

public class GameLibaryFilter
{
    public string TitleFilter { get; set; } = "";
    public List<PlayStatus> PlayStatusFilters { get; set; } = new() { PlayStatus.Playing, PlayStatus.NotPlayed };
    public PropertyInfo SortProperty { get; set; } = typeof(LocalGame).GetProperty(nameof(LocalGame.LastPlayed))!;
    public GameLibaryFilterSortOrder SortOrder { get; set; } = GameLibaryFilterSortOrder.Descending;
}

public enum GameLibaryFilterSortOrder
{
    Ascending,
    Descending
}