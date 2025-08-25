using System.Text.RegularExpressions;

using GameManager.UI.Features.OnlineSearch;
using GameManager.UI.Features.OnlineSearch.Actions.SearcbF95Games;

namespace GameManager.UI.Features.GameArchiveImporter.Actions.SeachForMatchingGame;

public record SearchForMatchingGameAction(string Path);

internal class SearchForMatchingGameActionEffect : Effect<SearchForMatchingGameAction>
{
    private readonly IDispatcher _dispatcher;
    private readonly NavigationManager _navigationManger;

    public SearchForMatchingGameActionEffect(IDispatcher dispatcher, NavigationManager navigationManger)
    {
        _dispatcher = dispatcher;
        _navigationManger = navigationManger;
    }

    public override Task HandleAsync(SearchForMatchingGameAction action, IDispatcher dispatcher)
    {
        var title = Path.GetFileNameWithoutExtension(action.Path);
        title = Regex.Match(title, "^[^\\d]*|^(.+)").Value;
        title = title.Replace("_", " ");
        title = title.Replace("-", " ");

        //remove any white space that might cause the ends with to fail
        title = title.Trim();
        
        //remove any terms that appear in the ProbablyNotTitle from the end of the title
        foreach (var term in Consts.ProbablyNotTitle)
        {
            if (title.EndsWith($" {term}", StringComparison.OrdinalIgnoreCase))
            {
                title = title[..^term.Length];
            }
        }
        
        //split the title with spaces at every capital letter
        title = Regex.Replace(title, "([a-z])([A-Z])", "$1 $2");
        
        title = title.Trim();

        var search = new F95SearchProperties
        {
            Term = title
        };

        _dispatcher.Dispatch(new SearchF95GamesAction(search));

        _dispatcher.Dispatch(new CloseModalAction());
        _navigationManger.NavigateTo("search");
        return Task.CompletedTask;
    }
}

