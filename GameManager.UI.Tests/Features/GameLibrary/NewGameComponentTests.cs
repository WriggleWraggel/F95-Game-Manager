using GameManager.UI.Features.GameLibrary.Components.NewGames;
using GameManager.UI.Features.GameLibrary.Actions.EditGame;

namespace GameManager.UI.Tests.Features.GameLibrary;

/// <summary>
/// Tests for NewGameComponent.
/// Verifies the scanning state, no-folders message, no-untracked-games message,
/// and that clicking Add on an unmanaged game dispatches OpenEditModalAction.
/// </summary>
public class NewGameComponentTests : BUnitTestBase
{
    [Fact]
    public void ShowsSearchingWhenScanning()
    {
        SetupState(new SettingsState { Settings = DefaultSettings(), Initialized = true });
        SetupState(new GameLibraryState { Scanning = true });

        var cut = RenderComponent<NewGameComponent>();

        cut.Markup.Should().Contain("Searching");
    }

    [Fact]
    public void ShowsNoGameFoldersMessageWhenNoLibraryFoldersConfigured()
    {
        var settings = new AppSettings
        {
            GameLibrarySettings = new GameLibrarySettings { Folders = new List<GameLibraryFolder>() }
        };
        SetupState(new SettingsState { Settings = settings, Initialized = true });
        SetupState(new GameLibraryState { Scanning = false, Games = new List<LocalGame>() });

        var cut = RenderComponent<NewGameComponent>();

        cut.Markup.Should().Contain("No game folders found");
    }

    [Fact]
    public void ShowsNoNewGamesMessageWhenAllGamesAreSaved()
    {
        SetupState(new SettingsState { Settings = DefaultSettings(), Initialized = true });
        SetupState(new GameLibraryState
        {
            Scanning = false,
            Games = new List<LocalGame> { CreateGame(saved: true) }
        });

        var cut = RenderComponent<NewGameComponent>();

        cut.Markup.Should().Contain("No new/untracked games found");
    }

    [Fact]
    public void RendersUnsavedGames()
    {
        SetupState(new SettingsState { Settings = DefaultSettings(), Initialized = true });
        SetupState(new GameLibraryState
        {
            Scanning = false,
            Games = new List<LocalGame>
            {
                CreateGame(title: "New Discovery", saved: false)
            }
        });

        var cut = RenderComponent<NewGameComponent>();

        cut.Markup.Should().Contain("New Discovery");
    }

    [Fact]
    public void ClickingAddDispatchesOpenEditModalAction()
    {
        var game = CreateGame(title: "Add Me", saved: false);
        SetupState(new SettingsState { Settings = DefaultSettings(), Initialized = true });
        SetupState(new GameLibraryState
        {
            Scanning = false,
            Games = new List<LocalGame> { game }
        });

        var cut = RenderComponent<NewGameComponent>();

        cut.Find("button").Click();

        DispatcherMock.Received().Dispatch(Arg.Is<OpenEditModalAction>(a => a.Game == game));
    }
}
