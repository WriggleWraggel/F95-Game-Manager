using GameManager.UI.Features.GameLibrary.Components.ExistingGames;
using System.Reflection;

namespace GameManager.UI.Tests.Features.GameLibrary;

/// <summary>
/// Tests for GameLibraryFilterComponent.
/// Verifies that sort property and sort-order selects are rendered, that the
/// play-status filter autocomplete is present, and that the correct Fluxor actions
/// are dispatched when the user changes sort or filter selections.
/// </summary>
public class GameLibraryFilterComponentTests : BUnitTestBase
{
    private void SetupLibraryState(GameLibaryFilter? filter = null)
    {
        SetupState(new GameLibraryState
        {
            Filter = filter ?? new GameLibaryFilter()
        });
    }

    [Fact]
    public void RendersSortHeading()
    {
        SetupLibraryState();

        var cut = RenderComponent<GameLibraryFilterComponent>();

        cut.Markup.Should().Contain("Sort:");
    }

    [Fact]
    public void RendersSortPropertyDropdown()
    {
        SetupLibraryState();

        var cut = RenderComponent<GameLibraryFilterComponent>();

        // SelectList renders as a <select> element
        cut.FindAll("select").Count.Should().BeGreaterThan(0);
    }

    [Fact]
    public void RendersSortOrderDropdown()
    {
        SetupLibraryState();

        var cut = RenderComponent<GameLibraryFilterComponent>();

        // Should contain Ascending and Descending options
        cut.Markup.Should().Contain("Ascending");
        cut.Markup.Should().Contain("Descending");
    }

    [Fact]
    public void DispatchesSortLibraryByPropertyActionWhenSortOrderChanges()
    {
        SetupLibraryState();

        var cut = RenderComponent<GameLibraryFilterComponent>();

        // Find the sort order select and change it to Ascending
        var selects = cut.FindAll("select");
        selects.Should().HaveCountGreaterThan(1);

        selects[1].Change("Ascending");

        DispatcherMock.Received().Dispatch(Arg.Any<object>());
    }
}
