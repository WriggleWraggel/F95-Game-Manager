using GameManager.UI.Features.GameUpdater.Components;
using GameManager.UI.Features.GameUpdater.Actions.GetUpdates;

namespace GameManager.UI.Tests.Features.GameUpdater;

/// <summary>
/// Tests for GameUpdaterTopBarComponent.
/// Verifies the different button states: not logged in, searching/loading,
/// no updates available (Check For Updates), and updates available.
/// Also verifies that GetF95UpdatesForLocalGamesAction is dispatched when
/// the Check For Updates button is clicked.
/// </summary>
public class GameUpdaterTopBarComponentTests : BUnitTestBase
{
    private void SetupStates(
        bool isLoggedIn = true,
        bool searching = false,
        bool haveUpdates = false)
    {
        var games = new List<LocalGame>();
        if ( haveUpdates )
        {
            var game = CreateGame(updateAvailable: true);
            game.F95Game = CreateF95Game();
            games.Add(game);
        }

        SetupState(new F95LoginState { IsLoggedIn = isLoggedIn });
        SetupState(new GameUpdaterState { SearchingForNewData = searching });
        SetupState(new GameLibraryState { Games = games });
    }

    [Fact]
    public void ShowsNotConnectedButtonWhenNotLoggedIn()
    {
        SetupStates(isLoggedIn: false);

        var cut = RenderComponent<GameUpdaterTopBarComponent>();

        cut.Markup.Should().Contain("Not connected to f95");
    }

    [Fact]
    public void ShowsCheckForUpdatesButtonWhenLoggedInAndNoUpdates()
    {
        SetupStates(isLoggedIn: true, searching: false, haveUpdates: false);

        var cut = RenderComponent<GameUpdaterTopBarComponent>();

        cut.Markup.Should().Contain("Check For Updates");
    }

    [Fact]
    public void ShowsUpdatingTextWhenSearchingForUpdates()
    {
        SetupStates(isLoggedIn: true, searching: true);

        var cut = RenderComponent<GameUpdaterTopBarComponent>();

        cut.Markup.Should().Contain("Updating");
    }

    [Fact]
    public void ShowsUpdateCountButtonWhenUpdatesAvailable()
    {
        SetupStates(isLoggedIn: true, searching: false, haveUpdates: true);

        var cut = RenderComponent<GameUpdaterTopBarComponent>();

        cut.Markup.Should().Contain("Game Updates");
    }

    [Fact]
    public void DispatchesGetF95UpdatesActionWhenCheckForUpdatesClicked()
    {
        SetupStates(isLoggedIn: true, searching: false, haveUpdates: false);

        var cut = RenderComponent<GameUpdaterTopBarComponent>();

        cut.Find("button").Click();

        DispatcherMock.Received().Dispatch(Arg.Any<GetF95UpdatesForLocalGamesAction>());
    }
}
