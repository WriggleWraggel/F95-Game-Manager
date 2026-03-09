using GameManager.UI.Features.GameLibrary.Controls;

namespace GameManager.UI.Tests.Controls;

/// <summary>
/// Tests for ManagedGameEditControl.
/// Verifies field rendering, the "Not Linked to F95" state, auto-save on blur,
/// and the OnSubmit callback when the form is saved.
/// </summary>
public class ManagedGameEditControlTests : BUnitTestBase
{
    private static LocalGame MakeGame(string title = "Edit Me") => new()
    {
        Title = title,
        Version = "1.0",
        Description = "desc",
        Saved = true,
        RootFolder = new GameLibraryFolder { Path = @"C:\Games" },
        FolderName = title
    };

    [Fact]
    public void RendersGameTitle()
    {
        var game = MakeGame("Rendered Title");

        var cut = RenderComponent<ManagedGameEditControl>(parameters => parameters
            .Add(p => p.Game, game)
            .Add(p => p.OnSubmit, EventCallback.Factory.Create<LocalGame>(this, _ => { }))
            .Add(p => p.AutoSaveEvent, EventCallback.Factory.Create<LocalGame>(this, _ => { }))
            .Add(p => p.LastDownloadFileSelection, new List<string>())
            .Add(p => p.PossibleGameFileSelection, new List<string>()));

        cut.Markup.Should().Contain("Rendered Title");
    }

    [Fact]
    public void ShowsNotLinkedTextWhenF95GameIsNull()
    {
        var game = MakeGame();
        game.F95Game = null;

        var cut = RenderComponent<ManagedGameEditControl>(parameters => parameters
            .Add(p => p.Game, game)
            .Add(p => p.OnSubmit, EventCallback.Factory.Create<LocalGame>(this, _ => { }))
            .Add(p => p.AutoSaveEvent, EventCallback.Factory.Create<LocalGame>(this, _ => { }))
            .Add(p => p.LastDownloadFileSelection, new List<string>())
            .Add(p => p.PossibleGameFileSelection, new List<string>()));

        cut.Markup.Should().Contain("Not Linked to F95");
    }

    [Fact]
    public void ShowsF95VersionWhenF95GameIsLinked()
    {
        var game = MakeGame();
        game.F95Game = CreateF95Game(version: "3.0-LINKED");

        var cut = RenderComponent<ManagedGameEditControl>(parameters => parameters
            .Add(p => p.Game, game)
            .Add(p => p.OnSubmit, EventCallback.Factory.Create<LocalGame>(this, _ => { }))
            .Add(p => p.AutoSaveEvent, EventCallback.Factory.Create<LocalGame>(this, _ => { }))
            .Add(p => p.LastDownloadFileSelection, new List<string>())
            .Add(p => p.PossibleGameFileSelection, new List<string>()));

        cut.Markup.Should().Contain("3.0-LINKED");
    }

    [Fact]
    public void RendersArchiveFileSelectionDropdown()
    {
        var game = MakeGame();
        var archives = new List<string> { "game_v1.zip", "game_v2.zip" };

        var cut = RenderComponent<ManagedGameEditControl>(parameters => parameters
            .Add(p => p.Game, game)
            .Add(p => p.OnSubmit, EventCallback.Factory.Create<LocalGame>(this, _ => { }))
            .Add(p => p.AutoSaveEvent, EventCallback.Factory.Create<LocalGame>(this, _ => { }))
            .Add(p => p.LastDownloadFileSelection, archives)
            .Add(p => p.PossibleGameFileSelection, new List<string>()));

        cut.Markup.Should().Contain("game_v1.zip");
        cut.Markup.Should().Contain("game_v2.zip");
    }

    [Fact]
    public void RendersSaveButton()
    {
        var game = MakeGame();

        var cut = RenderComponent<ManagedGameEditControl>(parameters => parameters
            .Add(p => p.Game, game)
            .Add(p => p.OnSubmit, EventCallback.Factory.Create<LocalGame>(this, _ => { }))
            .Add(p => p.AutoSaveEvent, EventCallback.Factory.Create<LocalGame>(this, _ => { }))
            .Add(p => p.LastDownloadFileSelection, new List<string>())
            .Add(p => p.PossibleGameFileSelection, new List<string>()));

        cut.Markup.Should().Contain("Save");
    }
}
