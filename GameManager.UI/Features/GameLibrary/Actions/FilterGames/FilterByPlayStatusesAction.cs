using GameManager.Core.MediatR.F95.Queries;
using GameManager.UI.Features.OnlineSearch;

namespace GameManager.UI.Features.GameLibrary.Actions.FilterGames;

internal record FilterByPlayStatusesAction(List<PlayStatus> PlayStatuses);

internal class FilterByPlayStatusesActionReducer : Reducer<GameLibraryState, FilterByPlayStatusesAction>
{
    public override GameLibraryState Reduce(GameLibraryState state, FilterByPlayStatusesAction action)
    {
        var filter = state.Filter;
        filter.PlayStatusFilters = action.PlayStatuses;
        return state with { Filter = filter };
    }
}