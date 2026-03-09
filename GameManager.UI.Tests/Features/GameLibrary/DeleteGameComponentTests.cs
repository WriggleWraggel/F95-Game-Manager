using GameManager.UI.Features.GameLibrary.Components.ExistingGames;
using GameManager.UI.Features.GameLibrary.Actions.DeleteGame;

namespace GameManager.UI.Tests.Features.GameLibrary;

/// <summary>
/// Tests for DeleteGameComponent.
/// Verifies that the delete option switches are rendered and that
/// DeleteGameAction is dispatched with the correct game when the form is submitted.
/// </summary>
public class DeleteGameComponentTests : BUnitTestBase
{
    [Fact]
    public void RendersDeleteOptionSwitches()
    {
        var game = CreateGame();
        SetupState(new GameLibraryState { Games = new List<LocalGame> { game } });

        var cut = RenderComponent<DeleteGameComponent>(parameters => parameters
            .Add(p => p.Game, game));

        cut.Markup.Should().Contain("Delete Archives");
        cut.Markup.Should().Contain("Delete Unarchived Game");
        cut.Markup.Should().Contain("Delete Mods");
        cut.Markup.Should().Contain("Delete Entire Game");
    }

    [Fact]
    public void RendersDeleteButton()
    {
        var game = CreateGame();
        SetupState(new GameLibraryState { Games = new List<LocalGame> { game } });

        var cut = RenderComponent<DeleteGameComponent>(parameters => parameters
            .Add(p => p.Game, game));

        cut.Markup.Should().Contain("Delete");
    }

    [Fact]
    public void DispatchesDeleteGameActionOnSubmit()
    {
        var game = CreateGame();
        SetupState(new GameLibraryState { Games = new List<LocalGame> { game } });

        var cut = RenderComponent<DeleteGameComponent>(parameters => parameters
            .Add(p => p.Game, game));

        cut.Find("form").Submit();

        DispatcherMock.Received().Dispatch(Arg.Is<DeleteGameAction>(a => a.Game == game));
    }
}
