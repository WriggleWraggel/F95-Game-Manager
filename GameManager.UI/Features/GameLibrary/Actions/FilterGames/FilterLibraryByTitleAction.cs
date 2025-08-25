using GameManager.Core.MediatR.F95.Queries;
using GameManager.UI.Features.OnlineSearch;

namespace GameManager.UI.Features.GameLibrary.Actions.FilterGames;

internal record FilterLibraryByTitleAction(string Title);

internal class FilterLibraryByTitleActionReducer : Reducer<GameLibraryState, FilterLibraryByTitleAction>
{
    public override GameLibraryState Reduce(GameLibraryState state, FilterLibraryByTitleAction action)
    {
        var filter = state.Filter;
        filter.TitleFilter = action.Title;
        return state with { Filter = filter };
    }
}