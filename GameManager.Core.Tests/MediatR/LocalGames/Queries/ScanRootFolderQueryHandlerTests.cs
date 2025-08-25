using GameManager.Core.MediatR.LocalGames.Queries;

namespace GameManager.Core.Tests.MediatR.LocalGames.Queries;

public class ScanRootFolderQueryHandlerTests
{
    [Fact]
    public async Task ReturnsLocalGamesFromRootFolder()
    {
        var hut = new ScanRootFolderQueryHandler();
        var res = await hut.Handle(new ScanRootFolderQuery { RootFolder = new GameLibraryFolder { Path = "D:\\HGames" } }, CancellationToken.None);
        res.Should().NotBeEmpty();

    }
}