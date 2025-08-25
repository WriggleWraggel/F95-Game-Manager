using GameManager.Core.MediatR.F95.Queries;
using GameManager.UI.Features.OnlineSearch.Actions.SearcbF95Games;

namespace GameManager.UI.Features.OnlineSearch.Actions;

public record SearchLocalGameOnF95Action(LocalGame LocalGame);

internal class SearchLocalGameOffF95ActionEffect : Effect<SearchLocalGameOnF95Action>
{
    private readonly IMediator _mediator;
    private readonly IState<OnlineSearchState> _searchState;

    public SearchLocalGameOffF95ActionEffect(IMediator mediator, IState<OnlineSearchState> searchState)
    {
        _mediator = mediator;
        _searchState = searchState;
    }

    public override async Task HandleAsync(SearchLocalGameOnF95Action action, IDispatcher dispatcher)
    {
        try
        {
            var res = await _mediator.Send(new SearchF95Query
            {
                Term = action.LocalGame.Title,
                Prefixes = _searchState.Value.Search.Prefixes,
                Tags = _searchState.Value.Search.Tags,
            });
            dispatcher.Dispatch(new SearchF95GamesSuccessAction(res));
        }
        catch ( Exception ex )
        {
            dispatcher.Dispatch(new AddErrorNotificationAction(ex.Message, ex, $"Error searching for {action.LocalGame.Title} on F95"));
        }
    }
}

internal class SearchLocalGameOnF95ActionReducer : Reducer<OnlineSearchState, SearchLocalGameOnF95Action>
{
    public override OnlineSearchState Reduce(OnlineSearchState state, SearchLocalGameOnF95Action action) =>
        state with
        {
            IsSearching = true,
            SelectedGame = action.LocalGame,
            Search = new F95SearchProperties
            {
                Term = action.LocalGame.Title,
                Tags = state.Search.Tags,
                Prefixes = state.Search.Prefixes,
            }
        };
}



