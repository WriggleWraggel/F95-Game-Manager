using Bunit.TestDoubles;

using IndexPage = GameManager.UI.Pages.Index;
using UpdatesPage = GameManager.UI.Pages.Updates;
using SearchPage = GameManager.UI.Pages.SearchPage;
using ScanFoldersPage = GameManager.UI.Pages.ScanFoldersPage;
using SettingsPage = GameManager.UI.Pages.SettingsPage;

namespace GameManager.UI.Tests.Pages;

/// <summary>
/// Tests for all application pages.
/// Verifies that each page renders its primary child components.
/// Pages are thin wrappers that compose feature components, so tests focus on
/// confirming the correct components are included in the rendered output.
/// </summary>
public class PageTests : BUnitTestBase
{
    private void SetupAllStates()
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

    // ── Index Page ────────────────────────────────────────────────────────────

    [Fact]
    public void IndexPage_RendersWithoutCrashing()
    {
        SetupAllStates();

        var cut = RenderComponent<IndexPage>();

        cut.Should().NotBeNull();
    }

    [Fact]
    public void IndexPage_ShowsNoLocalGamesWhenLibraryIsEmpty()
    {
        SetupAllStates();

        var cut = RenderComponent<IndexPage>();

        cut.Markup.Should().Contain("No Local Games");
    }

    // ── Updates Page ─────────────────────────────────────────────────────────

    [Fact]
    public void UpdatesPage_RendersWithoutCrashing()
    {
        SetupAllStates();

        var cut = RenderComponent<UpdatesPage>();

        cut.Should().NotBeNull();
    }

    [Fact]
    public void UpdatesPage_RendersGameGrid()
    {
        SetupAllStates();

        var cut = RenderComponent<UpdatesPage>();

        // game-grid div is always rendered by GameUpdaterComponent
        cut.Markup.Should().Contain("game-grid");
    }

    // ── Search Page ──────────────────────────────────────────────────────────

    [Fact]
    public void SearchPage_RendersWithoutCrashing()
    {
        SetupAllStates();

        var cut = RenderComponent<SearchPage>();

        cut.Should().NotBeNull();
    }

    [Fact]
    public void SearchPage_ContainsSearchHeading()
    {
        SetupAllStates();

        var cut = RenderComponent<SearchPage>();

        cut.Markup.Should().Contain("Search");
    }

    [Fact]
    public void SearchPage_RendersSearchForm()
    {
        SetupAllStates();

        var cut = RenderComponent<SearchPage>();

        // F95SearchComponent renders a form
        cut.FindAll("form").Should().NotBeEmpty();
    }

    // ── Scan Folders Page ────────────────────────────────────────────────────

    [Fact]
    public void ScanFoldersPage_RendersWithoutCrashing()
    {
        SetupAllStates();

        var cut = RenderComponent<ScanFoldersPage>();

        cut.Should().NotBeNull();
    }

    [Fact]
    public void ScanFoldersPage_RendersLibraryFoldersCard()
    {
        SetupAllStates();

        var cut = RenderComponent<ScanFoldersPage>();

        cut.Markup.Should().Contain("Library Folders");
    }

    // ── Settings Page ────────────────────────────────────────────────────────

    [Fact]
    public void SettingsPage_RendersWithoutCrashing()
    {
        SetupAllStates();

        var cut = RenderComponent<SettingsPage>();

        cut.Should().NotBeNull();
    }

    [Fact]
    public void SettingsPage_RendersSettingsHeading()
    {
        SetupAllStates();

        var cut = RenderComponent<SettingsPage>();

        cut.Markup.Should().Contain("Settings");
    }

    [Fact]
    public void SettingsPage_RendersF95AuthCard()
    {
        SetupAllStates();

        var cut = RenderComponent<SettingsPage>();

        cut.Markup.Should().Contain("F95 Auth Details");
    }

    [Fact]
    public void SettingsPage_RendersSevenZipCard()
    {
        SetupAllStates();

        var cut = RenderComponent<SettingsPage>();

        cut.Markup.Should().Contain("7Zip Install Location");
    }
}
