using SearchPage = GameManager.UI.Pages.SearchPage;

namespace GameManager.UI.Tests.Pages;

/// <summary>
/// Tests for the Search page (/search).
/// The Search page renders F95SearchComponent (search form) and
/// F95SearchResultsComponent (paginated results), allowing the user to
/// search F95zone and add or bind games to the local library.
/// </summary>
public class SearchPageTests : BUnitTestBase
{
    public SearchPageTests()
    {
        SetupState(new GameLibraryState());
        SetupState(new SettingsState { Settings = DefaultSettings(), Initialized = true });
        SetupState(new OnlineSearchState
        {
            Search = new F95SearchProperties(),
            Result = new F95SearchResult()
        });
        SetupState(new F95LoginState { IsLoggedIn = true });
        SetupState(new GameUpdaterState());
        SetupState(new GameArchiveImporterState());
        SetupState(new ModalState
        {
            ModalBody = typeof(GameManager.UI.Features.Modal.Components.ModalBodyPlaceHolder),
            Parameters = new Dictionary<string, object>()
        });
    }

    [Fact]
    public void RendersWithoutCrashing()
    {
        var cut = RenderComponent<SearchPage>();

        cut.Should().NotBeNull();
    }

    [Fact]
    public void ContainsSearchHeading()
    {
        var cut = RenderComponent<SearchPage>();

        cut.Markup.Should().Contain("Search");
    }

    [Fact]
    public void RendersSearchForm()
    {
        var cut = RenderComponent<SearchPage>();

        // F95SearchComponent renders a form
        cut.FindAll("form").Should().NotBeEmpty();
    }
}
