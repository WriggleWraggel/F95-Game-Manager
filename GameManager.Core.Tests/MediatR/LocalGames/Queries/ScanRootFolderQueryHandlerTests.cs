using System.IO;
using GameManager.Core.MediatR.LocalGames.Queries;

namespace GameManager.Core.Tests.MediatR.LocalGames.Queries;

public class ScanRootFolderQueryHandlerTests : IDisposable
{
    private readonly string _testRootFolder;

    public ScanRootFolderQueryHandlerTests()
    {
        _testRootFolder = Path.Combine(Path.GetTempPath(), "F95GameManagerTests", Guid.NewGuid().ToString());
        Directory.CreateDirectory(Path.Combine(_testRootFolder, "TestGame1"));
        Directory.CreateDirectory(Path.Combine(_testRootFolder, "TestGame2"));
    }

    [Fact]
    public async Task ReturnsLocalGamesFromRootFolder()
    {
        var hut = new ScanRootFolderQueryHandler();
        var res = await hut.Handle(new ScanRootFolderQuery { RootFolder = new GameLibraryFolder { Path = _testRootFolder } }, CancellationToken.None);
        res.Should().NotBeEmpty();

    }

    public void Dispose()
    {
        if (Directory.Exists(_testRootFolder))
            Directory.Delete(_testRootFolder, true);
    }
}