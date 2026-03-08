using GameManager.UI.Features.GameLibrary.Components.ExistingGames;

namespace GameManager.UI.Tests.Features.GameLibrary;

/// <summary>
/// Tests for ExistingGamesListComponent.
/// Verifies the three display states: scanning indicator, "No Local Games" message,
/// and the game grid when saved games are present. Also verifies that unsaved games
/// are excluded from the rendered list.
/// </summary>
public class ExistingGamesListComponentTests : BUnitTestBase
{
    private void SetupSettingsState(AppSettings? settings = null)
    {
        SetupState(new SettingsState { Settings = settings ?? DefaultSettings(), Initialized = true });
    }

    [Fact]
    public void ShowsSearchingWhenScanning()
    {
        SetupSettingsState();
        SetupState(new GameLibraryState { Scanning = true });

        var cut = RenderComponent<ExistingGamesListComponent>();

        cut.Markup.Should().Contain("Searching");
    }

    [Fact]
    public void ShowsNoLocalGamesWhenNoPersistableGames()
    {
        SetupSettingsState();
        SetupState(new GameLibraryState
        {
            Scanning = false,
            Games = new List<LocalGame>()
        });

        var cut = RenderComponent<ExistingGamesListComponent>();

        cut.Markup.Should().Contain("No Local Games");
    }

    [Fact]
    public void ShowsNoLocalGamesWhenAllGamesAreUnsaved()
    {
        SetupSettingsState();
        SetupState(new GameLibraryState
        {
            Scanning = false,
            Games = new List<LocalGame> { CreateGame(saved: false) }
        });

        var cut = RenderComponent<ExistingGamesListComponent>();

        cut.Markup.Should().Contain("No Local Games");
    }

    [Fact]
    public void RendersGameGridWhenSavedGamesExist()
    {
        SetupSettingsState();
        SetupState(new GameLibraryState
        {
            Scanning = false,
            Games = new List<LocalGame>
            {
                CreateGame(title: "Alpha Game", saved: true),
                CreateGame(title: "Beta Game", saved: true)
            }
        });

        var cut = RenderComponent<ExistingGamesListComponent>();

        cut.Markup.Should().Contain("Alpha Game");
        cut.Markup.Should().Contain("Beta Game");
    }

    [Fact]
    public void ExcludesUnsavedGamesFromGrid()
    {
        SetupSettingsState();
        SetupState(new GameLibraryState
        {
            Scanning = false,
            Games = new List<LocalGame>
            {
                CreateGame(title: "Saved Game", saved: true),
                CreateGame(title: "Unsaved Game", saved: false)
            }
        });

        var cut = RenderComponent<ExistingGamesListComponent>();

        cut.Markup.Should().Contain("Saved Game");
        cut.Markup.Should().NotContain("Unsaved Game");
    }
}
