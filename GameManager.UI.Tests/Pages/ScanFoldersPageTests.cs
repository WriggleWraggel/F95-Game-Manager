using ScanFoldersPage = GameManager.UI.Pages.ScanFoldersPage;

namespace GameManager.UI.Tests.Pages;

/// <summary>
/// Tests for the Scan Folders page (/libraryFolders).
/// The Scan Folders page renders LibraryFoldersComponent for managing folder paths
/// and triggering scans, and NewGameComponent to display discovered but unsaved games.
/// </summary>
public class ScanFoldersPageTests : BUnitTestBase
{
    public ScanFoldersPageTests()
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
        var cut = RenderComponent<ScanFoldersPage>();

        cut.Should().NotBeNull();
    }

    [Fact]
    public void RendersLibraryFoldersCard()
    {
        var cut = RenderComponent<ScanFoldersPage>();

        cut.Markup.Should().Contain("Library Folders");
    }
}
