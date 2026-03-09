using GameManager.UI.Controls;

namespace GameManager.UI.Tests.Controls;

/// <summary>
/// Tests for GamesScreenControl.
/// Verifies that images are rendered for each provided URL and that the control
/// renders nothing meaningful when the URL list is empty.
/// </summary>
public class GamesScreenControlTests : BUnitTestBase
{
    [Fact]
    public void RendersOneImagePerUrl()
    {
        var urls = new List<Flurl.Url>
        {
            new Flurl.Url("https://example.com/img1.jpg"),
            new Flurl.Url("https://example.com/img2.jpg"),
            new Flurl.Url("https://example.com/img3.jpg")
        };

        var cut = RenderComponent<GamesScreenControl>(parameters => parameters
            .Add(p => p.ImageUrls, urls));

        // Each image URL renders a col div
        cut.FindAll(".col").Count.Should().Be(3);
    }

    [Fact]
    public void RendersEmptyWhenNoUrls()
    {
        var cut = RenderComponent<GamesScreenControl>(parameters => parameters
            .Add(p => p.ImageUrls, new List<Flurl.Url>()));

        cut.FindAll(".col").Count.Should().Be(0);
    }
}
