using GameManager.UI.Features.OnlineSearch.Components;

namespace GameManager.UI.Tests.Features.OnlineSearch;

/// <summary>
/// Tests for F95SearchResultsComponent.
/// Verifies the searching state, the no-results message, and the game grid when
/// results are available including the Add Game / Bind to Game button text.
/// </summary>
public class F95SearchResultsComponentTests : BUnitTestBase
{
    private void SetupDefaultDependencies(
        OnlineSearchState? searchState = null,
        GameLibraryState? libraryState = null,
        AppSettings? settings = null)
    {
        SetupState(searchState ?? new OnlineSearchState());
        SetupState(libraryState ?? new GameLibraryState());
        SetupState(new SettingsState { Settings = settings ?? DefaultSettings(), Initialized = true });
    }

    [Fact]
    public void ShowsSearchingWhenIsSearchingIsTrue()
    {
        SetupDefaultDependencies(searchState: new OnlineSearchState { IsSearching = true });

        var cut = RenderComponent<F95SearchResultsComponent>();

        cut.Markup.Should().Contain("Searching");
    }

    [Fact]
    public void ShowsNoResultsFoundWhenResultListIsEmpty()
    {
        SetupDefaultDependencies(searchState: new OnlineSearchState
        {
            IsSearching = false,
            Result = new F95SearchResult { Body = new F95SearchResultBody { Games = new List<F95Game>() } }
        });

        var cut = RenderComponent<F95SearchResultsComponent>();

        cut.Markup.Should().Contain("No Results found");
    }

    [Fact]
    public void RendersGameCardWhenResultsPresent()
    {
        var f95Game = CreateF95Game(id: 42, title: "Search Result Game");
        SetupDefaultDependencies(
            searchState: new OnlineSearchState
            {
                IsSearching = false,
                Result = new F95SearchResult
                {
                    Body = new F95SearchResultBody
                    {
                        Games = new List<F95Game> { f95Game },
                        Pagination = new F95Pagination { Page = 1, Total = 1 }
                    }
                }
            },
            libraryState: new GameLibraryState { Games = new List<LocalGame>() });

        var cut = RenderComponent<F95SearchResultsComponent>();

        cut.Markup.Should().Contain("Search Result Game");
    }

    [Fact]
    public void ShowsAddGameButtonWhenNoLocalGameSelected()
    {
        var f95Game = CreateF95Game(id: 99, title: "New Title");
        SetupDefaultDependencies(
            searchState: new OnlineSearchState
            {
                IsSearching = false,
                SelectedGame = null,
                Result = new F95SearchResult
                {
                    Body = new F95SearchResultBody
                    {
                        Games = new List<F95Game> { f95Game },
                        Pagination = new F95Pagination { Page = 1, Total = 1 }
                    }
                }
            },
            libraryState: new GameLibraryState { Games = new List<LocalGame>() });

        var cut = RenderComponent<F95SearchResultsComponent>();

        cut.Markup.Should().Contain("Add Game");
    }

    [Fact]
    public void ShowsBindButtonTextWhenLocalGameIsSelected()
    {
        var f95Game = CreateF95Game(id: 100, title: "Bind Target");
        var localGame = CreateGame(title: "Local Bound Game");

        SetupDefaultDependencies(
            searchState: new OnlineSearchState
            {
                IsSearching = false,
                SelectedGame = localGame,
                Result = new F95SearchResult
                {
                    Body = new F95SearchResultBody
                    {
                        Games = new List<F95Game> { f95Game },
                        Pagination = new F95Pagination { Page = 1, Total = 1 }
                    }
                }
            },
            libraryState: new GameLibraryState { Games = new List<LocalGame>() });

        var cut = RenderComponent<F95SearchResultsComponent>();

        cut.Markup.Should().Contain("Bind to Local Bound Game");
    }
}
