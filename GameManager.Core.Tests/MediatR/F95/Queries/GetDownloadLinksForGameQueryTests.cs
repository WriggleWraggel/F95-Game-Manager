using GameManager.Core.Data.F95;
using GameManager.Core.MediatR.F95.Commands;
using GameManager.Core.MediatR.F95.Queries;

using Microsoft.Extensions.Logging;

namespace GameManager.Core.Tests.MediatR.F95.Queries;



public class GetDownloadLinksForGameQueryHandlerTests
{
    private readonly HttpSessionWrapper _wrapper = new();
    public GetDownloadLinksForGameQueryHandlerTests()
    {
        var auth = new AuthF95CommandHandler(_wrapper, Substitute.For<ILogger<AuthF95CommandHandler>>());
        auth.Handle(new AuthF95Command
        {
            Username = "",
            Password = ""
        }, CancellationToken.None).Wait();
    }

    [Theory]
    //[InlineData(93557, 11)]
    [InlineData(1878, 15)]
    public async Task GetDownloadLinks(int id, int expectCount)
    {
        var hut = new GetF95DownloadLinksForGameQueryHandler(_wrapper);
        var res = await hut.Handle(new GetF95DownloadLinksForGameQuery
        {
            F95Game = new F95Game { Id = id }
        }, CancellationToken.None);

        res.Should().HaveCount(expectCount);
    }

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