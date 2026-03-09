using GameManager.UI.Features.GameUpdater.Controls;

namespace GameManager.UI.Tests.Controls;

/// <summary>
/// Tests for UpdateableGameControl.
/// Verifies that game title, local/F95 version badges, and date information are
/// rendered, and that the cover background image is set from the linked F95 game.
/// </summary>
public class UpdateableGameControlTests : BUnitTestBase
{
    [Fact]
    public void RendersGameTitle()
    {
        var game = CreateGame(title: "Updatable Game");

        var cut = RenderComponent<UpdateableGameControl>(parameters => parameters
            .Add(p => p.Game, game)
            .Add(p => p.Buttons, "<span></span>"));

        cut.Markup.Should().Contain("Updatable Game");
    }

    [Fact]
    public void RendersLocalVersionBadge()
    {
        var game = CreateGame(version: "1.0");

        var cut = RenderComponent<UpdateableGameControl>(parameters => parameters
            .Add(p => p.Game, game)
            .Add(p => p.Buttons, "<span></span>"));

        cut.Markup.Should().Contain("1.0");
    }

    [Fact]
    public void RendersF95VersionBadgeWhenLinked()
    {
        var game = CreateGame(version: "1.0");
        game.F95Game = CreateF95Game(version: "2.0-f95");

        var cut = RenderComponent<UpdateableGameControl>(parameters => parameters
            .Add(p => p.Game, game)
            .Add(p => p.Buttons, "<span></span>"));

        cut.Markup.Should().Contain("2.0-f95");
    }

    [Fact]
    public void RendersF95UpdateDateWhenLinked()
    {
        var game = CreateGame();
        game.F95Game = CreateF95Game();
        game.F95Game.ThreadLastUpdatedDate = new DateTime(2024, 6, 15);

        var cut = RenderComponent<UpdateableGameControl>(parameters => parameters
            .Add(p => p.Game, game)
            .Add(p => p.Buttons, "<span></span>"));

        cut.Markup.Should().Contain("F95 Updated Date");
    }

    [Fact]
    public void RendersCoverBackgroundImageFromF95Game()
    {
        var game = CreateGame();
        game.F95Game = CreateF95Game();
        game.F95Game.CoverUrl = new Flurl.Url("https://example.com/cover.jpg");

        var cut = RenderComponent<UpdateableGameControl>(parameters => parameters
            .Add(p => p.Game, game)
            .Add(p => p.Buttons, "<span></span>"));

        cut.Markup.Should().Contain("background-image");
    }
}
