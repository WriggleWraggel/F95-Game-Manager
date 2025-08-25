using GameManager.Core.MediatR.F95.Queries;

namespace GameManager.UI.Features.OnlineSearch.Actions.SaveSearchResult;

public record SaveSearchResultAction(F95Game SearchResult, LocalGame LocalGame);

internal class SaveSearchResultActionEffect : Effect<SaveSearchResultAction>
{
    private readonly IMediator _mediator;

    public SaveSearchResultActionEffect(IMediator mediator) => _mediator = mediator;

    public override async Task HandleAsync(SaveSearchResultAction action, IDispatcher dispatcher)
    {
        var localGame = action.LocalGame;
        var searchResult = action.SearchResult;
        var summary = new F95SummaryData();

        try
        {
            summary = await _mediator.Send(new GetF95PageSummaryDataQuery(action.SearchResult));
        }
        catch ( Exception ex )
        {
            dispatcher.Dispatch(new AddErrorNotificationAction(
                $"Error getting summary data for {searchResult.Title}",
                ex,
                "Error getting additional details for search result"));
        }

        //bind action
        if ( localGame.F95Game == null )
        {
            localGame.F95Game = searchResult;
            if ( string.IsNullOrWhiteSpace(localGame.Version) )
            {
                //set the version based of the page data and not the api
                localGame.Version = summary.Version ?? searchResult.Version;
                localGame.F95Game.Version = summary.Version ?? searchResult.Version;
            }
            localGame.CoverUrl = searchResult.CoverUrl;
            localGame.Title = searchResult.Title;
            localGame.ScreenUrls = searchResult.Screens;
            localGame.Creator = searchResult.Creator;
            localGame.CustomSearchTerm = localGame.Title;
            localGame.GameEngine = searchResult.GameEngine;
            localGame.GameUrl = searchResult.GameUrl;
        }

        localGame.F95Game.ThreadLastUpdatedDate = summary.UpdateDate;

        dispatcher.Dispatch(new SaveLocalGameAction(action.LocalGame));
        dispatcher.Dispatch(new SaveSearchResultSuccessAction());
        //rescan downloads to look for any archives that belong to the new game
        dispatcher.Dispatch(new ScanAllDownloadFoldersAction());
    }
}

