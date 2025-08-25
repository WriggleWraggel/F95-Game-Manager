using GameManager.Core.MediatR.F95.Queries;

namespace GameManager.UI.Features.OnlineSearch.Actions.SearcbF95Games;

internal class SearchF95GamesFailureAction { }

internal class SearchF95GamesFailureActionReducer : Reducer<OnlineSearchState, SearchF95GamesFailureAction>
{
    public override OnlineSearchState Reduce(OnlineSearchState state, SearchF95GamesFailureAction action) =>
        state with
        {
            IsSearching = false,
            Result = new F95SearchResult()
        };
}
