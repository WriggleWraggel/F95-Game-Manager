using GameManager.UI.Controls;

namespace GameManager.UI.Tests.Controls;

/// <summary>
/// Tests for GameSummaryControl.
/// Verifies that game title, engine, version badges and cover image are rendered
/// correctly, and that the overlay is shown or hidden based on the ShowOverlay parameter.
/// </summary>
public class GameSummaryControlTests : BUnitTestBase
{
    [Fact]
    public void RendersGameTitle()
    {
        var game = CreateGame(title: "My Test Game");

        var cut = RenderComponent<GameSummaryControl>(parameters => parameters
            .Add(p => p.Game, game)
            .Add(p => p.ExtraLeftBadges, "<span></span>")
            .Add(p => p.ExtraRightBadges, "<span></span>")
            .Add(p => p.Buttons, "<span></span>"));

        cut.Markup.Should().Contain("My Test Game");
    }

    [Fact]
    public void RendersVersionBadge()
    {
        var game = CreateGame(version: "v1.5");

        var cut = RenderComponent<GameSummaryControl>(parameters => parameters
            .Add(p => p.Game, game)
            .Add(p => p.ExtraLeftBadges, "<span></span>")
            .Add(p => p.ExtraRightBadges, "<span></span>")
            .Add(p => p.Buttons, "<span></span>"));

        cut.Markup.Should().Contain("v1.5");
    }

    [Fact]
    public void RendersEngineAsBadge()
    {
        var game = CreateGame();
        game.GameEngine = GameEngine.Renpy;

        var cut = RenderComponent<GameSummaryControl>(parameters => parameters
            .Add(p => p.Game, game)
            .Add(p => p.ExtraLeftBadges, "<span></span>")
            .Add(p => p.ExtraRightBadges, "<span></span>")
            .Add(p => p.Buttons, "<span></span>"));

        cut.Markup.Should().Contain("Renpy");
    }

    [Fact]
    public void ShowsOverlayWhenShowOverlayIsTrue()
    {
        var game = CreateGame();

        var cut = RenderComponent<GameSummaryControl>(parameters => parameters
            .Add(p => p.Game, game)
            .Add(p => p.ShowOverlay, true)
            .Add(p => p.BackgroundOverlay, "<div class=\"test-overlay\">overlay</div>")
            .Add(p => p.ExtraLeftBadges, "<span></span>")
            .Add(p => p.ExtraRightBadges, "<span></span>")
            .Add(p => p.Buttons, "<span></span>"));

        cut.Markup.Should().Contain("cover-overlay");
    }

    [Fact]
    public void HidesOverlayWhenShowOverlayIsFalse()
    {
        var game = CreateGame();

        var cut = RenderComponent<GameSummaryControl>(parameters => parameters
            .Add(p => p.Game, game)
            .Add(p => p.ShowOverlay, false)
            .Add(p => p.ExtraLeftBadges, "<span></span>")
            .Add(p => p.ExtraRightBadges, "<span></span>")
            .Add(p => p.Buttons, "<span></span>"));

        cut.Markup.Should().NotContain("cover-overlay");
    }

    [Fact]
    public void SetsCoverBackgroundImageWhenCoverUrlIsSet()
    {
        var game = CreateGame();
        game.CoverUrl = new Flurl.Url("https://example.com/cover.jpg");

        var cut = RenderComponent<GameSummaryControl>(parameters => parameters
            .Add(p => p.Game, game)
            .Add(p => p.ExtraLeftBadges, "<span></span>")
            .Add(p => p.ExtraRightBadges, "<span></span>")
            .Add(p => p.Buttons, "<span></span>"));

        cut.Markup.Should().Contain("background-image");
    }

    [Fact]
    public void DoesNotSetCoverBackgroundImageWhenCoverUrlIsEmpty()
    {
        var game = CreateGame();
        game.CoverUrl = new Flurl.Url("");

        var cut = RenderComponent<GameSummaryControl>(parameters => parameters
            .Add(p => p.Game, game)
            .Add(p => p.ExtraLeftBadges, "<span></span>")
            .Add(p => p.ExtraRightBadges, "<span></span>")
            .Add(p => p.Buttons, "<span></span>"));

        cut.Find(".cover").GetAttribute("style").Should().BeNullOrEmpty();
    }
}
