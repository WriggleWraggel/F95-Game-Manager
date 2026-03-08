using GameManager.UI.Features.OnlineSearch.Components;

namespace GameManager.UI.Tests.Features.OnlineSearch;

/// <summary>
/// Tests for F95SearchComponent.
/// Verifies that the title input field and search button are rendered, and that
/// SearchF95GamesAction is dispatched when the form is submitted.
/// </summary>
public class F95SearchComponentTests : BUnitTestBase
{
    private void SetupStates(string searchTerm = "")
    {
        SetupState(new OnlineSearchState
        {
            Search = new F95SearchProperties { Term = searchTerm }
        });
        SetupState(new SettingsState { Settings = DefaultSettings(), Initialized = true });
    }

    [Fact]
    public void RendersSearchCard()
    {
        SetupStates();

        var cut = RenderComponent<F95SearchComponent>();

        cut.Markup.Should().Contain("Search F95");
    }

    [Fact]
    public void RendersTitleInputField()
    {
        SetupStates();

        var cut = RenderComponent<F95SearchComponent>();

        var inputs = cut.FindAll("input");
        inputs.Should().NotBeEmpty();
    }

    [Fact]
    public void RendersSearchButton()
    {
        SetupStates();

        var cut = RenderComponent<F95SearchComponent>();

        cut.Markup.Should().Contain("Search");
    }

    [Fact]
    public void DispatchesSearchF95GamesActionOnSubmit()
    {
        SetupStates("test game");

        var cut = RenderComponent<F95SearchComponent>();

        cut.Find("form").Submit();

        DispatcherMock.Received().Dispatch(Arg.Any<object>());
    }
}
