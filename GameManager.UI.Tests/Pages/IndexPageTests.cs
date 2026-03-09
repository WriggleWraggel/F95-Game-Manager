using IndexPage = GameManager.UI.Pages.Index;

namespace GameManager.UI.Tests.Pages;

/// <summary>
/// Tests for the Index page (/).
/// The Index page is the main game library dashboard that renders the
/// ExistingGamesListComponent to display all locally tracked games.
/// </summary>
public class IndexPageTests : BUnitTestBase
{
    public IndexPageTests()
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
        var cut = RenderComponent<IndexPage>();

        cut.Should().NotBeNull();
    }

    [Fact]
    public void ShowsNoLocalGamesWhenLibraryIsEmpty()
    {
        var cut = RenderComponent<IndexPage>();

        cut.Markup.Should().Contain("No Local Games");
    }
}
