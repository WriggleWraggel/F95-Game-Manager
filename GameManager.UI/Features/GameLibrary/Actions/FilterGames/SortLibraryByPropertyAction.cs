using System.Reflection;

using GameManager.Core.MediatR.F95.Queries;
using GameManager.UI.Features.OnlineSearch;

namespace GameManager.UI.Features.GameLibrary.Actions.FilterGames;

internal record SortLibraryByPropertyAction(PropertyInfo Property, GameLibaryFilterSortOrder SortOrder);

internal class SortLibraryByPropertyActionReducer : Reducer<GameLibraryState, SortLibraryByPropertyAction>
{
    public override GameLibraryState Reduce(GameLibraryState state, SortLibraryByPropertyAction action)
    {
        var filter = state.Filter;
        filter.SortProperty = action.Property;
        filter.SortOrder = action.SortOrder;
        return state with { Filter = filter };
    }
}