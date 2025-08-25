namespace GameManager.UI.Features.OnlineSearch.Actions.SaveSearchResult;

public record SaveSearchResultSuccessAction();

internal class SaveSearchResultSuccessActionReducer : Reducer<OnlineSearchState, SaveSearchResultSuccessAction>
{
    public override OnlineSearchState Reduce(OnlineSearchState state, SaveSearchResultSuccessAction action)
        => state with
        {
            SelectedGame = null,
            Search = new F95SearchProperties()
        };
}
