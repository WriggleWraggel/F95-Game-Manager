using GameManager.Core.Data.F95;
using GameManager.Core.MediatR.F95.Queries;

namespace GameManager.Core.Tests.MediatR.F95.Queries;

public class GetF95PageSummaryDataQueryTests
{
    [IntegrationFact]
    public async Task GetsSummaryData()
    {
        var hut = new GetF95PageSummaryDataQueryHandler(new F95Client());
        var res = await hut.Handle(new GetF95PageSummaryDataQuery(
            new F95Game { Id = 1878 })
            , CancellationToken.None);

        res.UpdateDate.Should().NotBeNull();
    }
}
