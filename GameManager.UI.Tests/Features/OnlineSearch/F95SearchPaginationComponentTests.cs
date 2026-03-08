using GameManager.UI.Features.OnlineSearch.Components;

namespace GameManager.UI.Tests.Features.OnlineSearch;

/// <summary>
/// Tests for F95SearchPaginationComponent.
/// Verifies page number rendering, that the Previous button is disabled on the first
/// page and Next is disabled on the last page, and that SearchF95GamesAction is
/// dispatched when a page number is clicked.
/// </summary>
public class F95SearchPaginationComponentTests : BUnitTestBase
{
    private void SetupPaginationState(int currentPage, int totalPages)
    {
        SetupState(new OnlineSearchState
        {
            Search = new F95SearchProperties { Page = currentPage },
            Result = new F95SearchResult
            {
                Body = new F95SearchResultBody
                {
                    Pagination = new F95Pagination { Page = currentPage, Total = totalPages }
                }
            }
        });
    }

    [Fact]
    public void RendersPageNumbers()
    {
        SetupPaginationState(currentPage: 1, totalPages: 3);

        var cut = RenderComponent<F95SearchPaginationComponent>();

        cut.Markup.Should().Contain("1");
        cut.Markup.Should().Contain("2");
        cut.Markup.Should().Contain("3");
    }

    [Fact]
    public void PreviousButtonIsDisabledOnFirstPage()
    {
        SetupPaginationState(currentPage: 1, totalPages: 3);

        var cut = RenderComponent<F95SearchPaginationComponent>();

        // The first pagination item should be disabled
        var firstPaginationItem = cut.Find("li.page-item");
        firstPaginationItem.ClassList.Should().Contain("disabled");
    }

    [Fact]
    public void NextButtonIsDisabledOnLastPage()
    {
        SetupPaginationState(currentPage: 3, totalPages: 3);

        var cut = RenderComponent<F95SearchPaginationComponent>();

        // The last pagination item should be disabled
        var items = cut.FindAll("li.page-item");
        items.Last().ClassList.Should().Contain("disabled");
    }

    [Fact]
    public void PreviousButtonIsEnabledOnNonFirstPage()
    {
        SetupPaginationState(currentPage: 2, totalPages: 3);

        var cut = RenderComponent<F95SearchPaginationComponent>();

        var firstItem = cut.Find("li.page-item");
        firstItem.ClassList.Should().NotContain("disabled");
    }

    [Fact]
    public void DispatchesSearchActionWhenPageNumberClicked()
    {
        SetupPaginationState(currentPage: 1, totalPages: 3);

        var cut = RenderComponent<F95SearchPaginationComponent>();

        // Click page 2 link
        var pageLinks = cut.FindAll("li.page-item a.page-link");
        // pageLinks[0]=Prev, [1]=page1, [2]=page2, [3]=page3, [4]=Next
        pageLinks[2].Click();

        DispatcherMock.Received().Dispatch(Arg.Any<object>());
    }
}
