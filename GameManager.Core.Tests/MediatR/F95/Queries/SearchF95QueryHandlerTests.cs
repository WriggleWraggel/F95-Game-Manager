using GameManager.Core.MediatR.F95.Queries;

namespace GameManager.Core.Tests.MediatR.F95.Queries;

public class SearchF95QueryHandlerTests
{
    [IntegrationFact]
    public async Task SearchReturnsAFullPageWithNoSearchTerm()
    {
        var wrapper = new HttpSessionWrapper();
        var hut = new SearchF95QueryHandler(wrapper);
        var res = await hut.Handle(new SearchF95Query(), CancellationToken.None);
        res.Body.Games.Count.Should().Be(30);
    }
}
