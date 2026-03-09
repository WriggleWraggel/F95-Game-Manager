using UpdatesPage = GameManager.UI.Pages.Updates;

namespace GameManager.UI.Tests.Pages;

/// <summary>
/// Tests for the Updates page (/updates).
/// The Updates page renders GameUpdaterComponent which displays all locally
/// tracked games that have a newer version available on F95zone.
/// </summary>
public class UpdatesPageTests : BUnitTestBase
{
    public UpdatesPageTests()
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
        var cut = RenderComponent<UpdatesPage>();

        cut.Should().NotBeNull();
    }

    [Fact]
    public void RendersGameGrid()
    {
        var cut = RenderComponent<UpdatesPage>();

        // game-grid div is always rendered by GameUpdaterComponent
        cut.Markup.Should().Contain("game-grid");
    }
}
