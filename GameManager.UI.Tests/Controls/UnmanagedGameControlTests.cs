using GameManager.UI.Features.GameLibrary.Controls;
using GameManager.UI.Features.GameLibrary.Actions.EditGame;

namespace GameManager.UI.Tests.Controls;

/// <summary>
/// Tests for UnmanagedGameControl.
/// Verifies that the game title and path are rendered, and that the AddGame
/// callback is invoked when the Add button is clicked.
/// </summary>
public class UnmanagedGameControlTests : BUnitTestBase
{
    [Fact]
    public void RendersGameTitle()
    {
        var game = CreateGame(title: "Untracked RPG", saved: false);

        var cut = RenderComponent<UnmanagedGameControl>(parameters => parameters
            .Add(p => p.Game, game)
            .Add(p => p.AddGame, EventCallback.Factory.Create<LocalGame>(this, _ => { })));

        cut.Markup.Should().Contain("Untracked RPG");
    }

    [Fact]
    public void RendersGameFullPath()
    {
        var folder = new GameLibraryFolder { Path = @"C:\Games" };
        var game = CreateGame(title: "PathGame", rootFolder: folder, saved: false);

        var cut = RenderComponent<UnmanagedGameControl>(parameters => parameters
            .Add(p => p.Game, game)
            .Add(p => p.AddGame, EventCallback.Factory.Create<LocalGame>(this, _ => { })));

        cut.Markup.Should().Contain(@"C:\Games");
    }

    [Fact]
    public async Task ClickingAddButtonInvokesAddGameCallback()
    {
        var game = CreateGame(title: "ClickGame", saved: false);
        LocalGame? capturedGame = null;

        var cut = RenderComponent<UnmanagedGameControl>(parameters => parameters
            .Add(p => p.Game, game)
            .Add(p => p.AddGame, EventCallback.Factory.Create<LocalGame>(this, g => capturedGame = g)));

        await cut.Find("button").ClickAsync(new Microsoft.AspNetCore.Components.Web.MouseEventArgs());

        capturedGame.Should().Be(game);
    }
}
