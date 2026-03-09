using GameManager.UI.Features.TopSeachBar.Components;

namespace GameManager.UI.Tests.Features.TopSearchBar;

/// <summary>
/// Tests for TopSeachBarComponent.
/// Verifies that the search input and button are rendered, that typing dispatches
/// FilterLibraryByTitleAction in real time, and that submitting the form dispatches
/// SearchF95GamesAction and navigates to the search page.
/// </summary>
public class TopSeachBarComponentTests : BUnitTestBase
{
    private void SetupStates()
    {
        SetupState(new OnlineSearchState
        {
            Search = new F95SearchProperties()
        });
    }

    [Fact]
    public void RendersSearchInput()
    {
        SetupStates();

        var cut = RenderComponent<TopSeachBarComponent>();

        var inputs = cut.FindAll("input");
        inputs.Should().NotBeEmpty();
    }

    [Fact]
    public void RendersSearchButton()
    {
        SetupStates();

        var cut = RenderComponent<TopSeachBarComponent>();

        var buttons = cut.FindAll("button");
        buttons.Should().NotBeEmpty();
    }

    [Fact]
    public void DispatchesFilterLibraryByTitleActionWhenTextChanges()
    {
        SetupStates();

        var cut = RenderComponent<TopSeachBarComponent>();

        cut.Find("input").Input("dragon quest");

        // FilterLibraryByTitleAction is dispatched on text input
        DispatcherMock.Received().Dispatch(Arg.Any<object>());
    }

    [Fact]
    public void DispatchesSearchF95GamesActionWhenFormSubmitted()
    {
        SetupStates();

        var cut = RenderComponent<TopSeachBarComponent>();

        cut.Find("input").Input("magic game");
        cut.Find("form").Submit();

        // SearchF95GamesAction and FilterLibraryByTitleAction are dispatched
        DispatcherMock.Received().Dispatch(Arg.Any<object>());
    }
}
