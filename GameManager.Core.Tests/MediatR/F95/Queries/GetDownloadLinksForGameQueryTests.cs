using GameManager.Core.Data.F95;
using GameManager.Core.MediatR.F95.Queries;

namespace GameManager.Core.Tests.MediatR.F95.Queries;

public class GetDownloadLinksForGameQueryHandlerTests
{
    [Fact]
    public void ErrorsWhenNoAuth()
    {
        var hut = new GetF95DownloadLinksForGameQueryHandler(new HttpSessionWrapper());

        Func<Task> act = async () => await hut.Handle(new GetF95DownloadLinksForGameQuery
        {
            F95Game = new F95Game { Id = 93557 }
        }, CancellationToken.None);

        act.Should().ThrowAsync<Exception>().WithMessage("Not Logged into f95");

    }
}