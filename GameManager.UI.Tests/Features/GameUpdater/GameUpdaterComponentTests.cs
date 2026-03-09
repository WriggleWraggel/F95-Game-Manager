using GameManager.UI.Features.GameUpdater.Components;
using GameManager.UI.Features.GameLibrary.Actions.SaveGame;
using GameManager.UI.Features.GameLibrary.Actions.EditGame;
using GameManager.UI.Features.IGames.Actions;

namespace GameManager.UI.Tests.Features.GameUpdater;

/// <summary>
/// Tests for GameUpdaterComponent.
/// Verifies that the game grid is rendered when update-available games exist,
/// and that the correct Fluxor actions are dispatched for Open Link, Edit, and
/// Ignore Update operations.
/// </summary>
public class GameUpdaterComponentTests : BUnitTestBase
{
    private void SetupStates(List<LocalGame>? games = null)
    {
        SetupState(new GameLibraryState
        {
            Games = games ?? new List<LocalGame>()
        });
        SetupState(new F95LoginState { IsLoggedIn = true });
        SetupState(new GameUpdaterState());
    }

    [Fact]
    public void RendersEmptyGridWhenNoUpdatableGames()
    {
        SetupStates(new List<LocalGame> { CreateGame(updateAvailable: false) });

        var cut = RenderComponent<GameUpdaterComponent>();

        // game grid is rendered but contains no game cards
        cut.FindAll(".game-card").Should().BeEmpty();
    }

    [Fact]
    public void RendersUpdatableGame()
    {
        var game = CreateGame(title: "Update Me", updateAvailable: true);
        game.F95Game = CreateF95Game(version: "2.0");
        SetupStates(new List<LocalGame> { game });

        var cut = RenderComponent<GameUpdaterComponent>();

        cut.Markup.Should().Contain("Update Me");
    }

    [Fact]
    public void DispatchesOpenGameInBrowserActionWhenOpenLinkClicked()
    {
        var game = CreateGame(title: "Browser Game", updateAvailable: true);
        game.F95Game = CreateF95Game();
        SetupStates(new List<LocalGame> { game });

        var cut = RenderComponent<GameUpdaterComponent>();

        // External link button uses fa-square-up-right icon in FontAwesome 6
        var linkButtons = cut.FindAll("button").Where(b => b.OuterHtml.Contains("fa-square-up-right")).ToList();
        linkButtons.Should().NotBeEmpty();
        linkButtons.First().Click();

        DispatcherMock.Received().Dispatch(Arg.Any<OpenGameInBrowserAction>());
    }

    [Fact]
    public void DispatchesOpenEditModalActionWhenEditClicked()
    {
        var game = CreateGame(title: "Edit Updatable", updateAvailable: true);
        game.F95Game = CreateF95Game();
        SetupStates(new List<LocalGame> { game });

        var cut = RenderComponent<GameUpdaterComponent>();

        // Edit button uses fa-pen-to-square icon in FontAwesome 6
        var editButtons = cut.FindAll("button").Where(b => b.OuterHtml.Contains("fa-pen-to-square")).ToList();
        editButtons.Should().NotBeEmpty();
        editButtons.First().Click();

        DispatcherMock.Received().Dispatch(Arg.Any<OpenEditModalAction>());
    }

    [Fact]
    public void DispatchesSaveLocalGameActionWhenIgnoreUpdateClicked()
    {
        var game = CreateGame(title: "Ignore Update", updateAvailable: true);
        game.F95Game = CreateF95Game();
        SetupStates(new List<LocalGame> { game });

        var cut = RenderComponent<GameUpdaterComponent>();

        var ignoreButton = cut.FindAll("button").FirstOrDefault(b => b.TextContent.Contains("Ignore"));
        ignoreButton.Should().NotBeNull();
        ignoreButton!.Click();

        DispatcherMock.Received().Dispatch(Arg.Any<SaveLocalGameAction>());
    }
}
