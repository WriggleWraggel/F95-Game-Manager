using System.IO;
using System.Reflection;

using GameManager.Core.MediatR.LocalGames.Queries;

namespace GameManager.Core.Tests.MediatR.LocalGames.Queries;

public class ScanRootFolderQueryHandlerTests : IDisposable
{
    private readonly string _rootFolder;
    private readonly string _gameSubFolder;

    public ScanRootFolderQueryHandlerTests()
    {
        var appPath = Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)!;
        _rootFolder = Path.Combine(appPath, "ScanRootFolderTest");
        _gameSubFolder = Path.Combine(_rootFolder, "TestGame");
        Directory.CreateDirectory(_gameSubFolder);
    }

    [Fact]
    public async Task ReturnsLocalGamesFromRootFolder()
    {
        var hut = new ScanRootFolderQueryHandler();
        var res = await hut.Handle(new ScanRootFolderQuery { RootFolder = new GameLibraryFolder { Path = _rootFolder } }, CancellationToken.None);
        res.Should().NotBeEmpty();
    }

    [Fact]
    public async Task ExcludesAlreadySavedGames()
    {
        // Create a subfolder that already has game data (simulates an already-saved game)
        var savedGameFolder = Path.Combine(_rootFolder, "SavedGame");
        Directory.CreateDirectory(savedGameFolder);
        await File.WriteAllTextAsync(Path.Combine(savedGameFolder, SettingsConsts.GameDataFileName), "{}");

        var hut = new ScanRootFolderQueryHandler();
        var res = await hut.Handle(new ScanRootFolderQuery { RootFolder = new GameLibraryFolder { Path = _rootFolder } }, CancellationToken.None);

        res.Should().NotContain(g => g.FolderName == "SavedGame");
    }

    public void Dispose()
    {
        if ( Directory.Exists(_rootFolder) )
            Directory.Delete(_rootFolder, true);
    }
}
