using GameManager.Core.MediatR.F95.Queries;

namespace GameManager.UI.Features.OnlineSearch.Actions.SearcbF95Games;

internal record SearchF95GamesAction(F95SearchProperties Search);

internal class SearchF95GamesActionEffect : Effect<SearchF95GamesAction>
{
    private readonly IMediator _mediator;

    public SearchF95GamesActionEffect(IMediator mediator) => _mediator = mediator;

    public override async Task HandleAsync(SearchF95GamesAction action, IDispatcher dispatcher)
    {
        try
        {
            var res = await _mediator.Send(new SearchF95Query
            {
                Page = action.Search.Page,
                Sort = action.Search.Sort,
                Term = action.Search.Term,
                Prefixes = action.Search.Prefixes,
                Tags = action.Search.Tags,
            });

            dispatcher.Dispatch(new SearchF95GamesSuccessAction(res));

        }
        catch ( Exception ex )
        {
            dispatcher.Dispatch(new AddErrorNotificationAction(ex.Message, ex, "Error searching f95"));
            dispatcher.Dispatch(new SearchF95GamesFailureAction());
        }
    }
}

internal class SearchF95GamesActionReducer : Reducer<OnlineSearchState, SearchF95GamesAction>
{
    public override OnlineSearchState Reduce(OnlineSearchState state, SearchF95GamesAction action) =>
        state with
        {
            IsSearching = true,
            Search = action.Search,
        };
}