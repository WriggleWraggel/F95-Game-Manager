using GameManager.Core.MediatR.F95.Queries;

namespace GameManager.UI.Features.OnlineSearch.Actions.SearcbF95Games;

internal record SearchF95GamesSuccessAction(F95SearchResult Result);

internal class SearchF95GamesSuccessActionReducer : Reducer<OnlineSearchState, SearchF95GamesSuccessAction>
{
    public override OnlineSearchState Reduce(OnlineSearchState state, SearchF95GamesSuccessAction action) =>
        state with
        {
            IsSearching = false,
            Result = action.Result
        };
}
