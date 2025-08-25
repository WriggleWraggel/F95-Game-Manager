using GameManager.Core.MediatR.F95.Queries;

namespace GameManager.UI.Features.OnlineSearch;

[FeatureState]
public record OnlineSearchState
{
    public F95SearchProperties Search { get; init; } = new();
    public F95SearchResult Result { get; init; } = new();
    public bool IsSearching { get; init; } = false;
    public LocalGame? SelectedGame { get; init; } = null;
}

public class F95SearchProperties
{
    public int Page { get; set; } = 1;
    public string Sort { get; set; } = "";
    public string Term { get; set; } = "";
    public List<F95GamePrefix> Prefixes { get; set; } = new();
    public List<F95Tag> Tags { get; set; } = new();

}
