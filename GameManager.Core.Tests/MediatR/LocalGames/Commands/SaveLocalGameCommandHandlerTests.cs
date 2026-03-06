using System.IO;
using System.Reflection;

using GameManager.Core.MediatR.LocalGames.Commands;
using GameManager.Core.Services;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GameManager.Core.Tests.MediatR.LocalGames.Commands;

public class SaveLocalGameCommandHandlerTests : IDisposable
{
    private NewtonSoftJsonSerializerWrapper _wrapper;
    private IFileRepo _fileRepo;
    private ILocalGameRepo _gameRepo;
    private string _folderName;

    public SaveLocalGameCommandHandlerTests()
    {
        _wrapper = new();
        _fileRepo = new FileRepo();
        _gameRepo = new LocalGameJsonFileRepo(_wrapper);
        _folderName = "testGameName";
    }

    [Fact]
    public async Task CreatesJsonFileAndSavesGameToIt()
    {
        var appPath = Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location);
        var filePath = Path.Join(appPath, _folderName, SettingsConsts.GameDataFileName);

        var game = new LocalGame
        {
            FolderName = _folderName,
            RootFolder = new GameLibraryFolder { Path = appPath! },
            Description = "testDes",
            CustomSearchTerm = "testSearchTerm",

            Title = "testGameTitle",
            F95Game = new Data.F95.F95Game
            {
                Id = 1,
                Title = "should appear",
                CoverUrl = "test cover",
                Screens = new List<Flurl.Url> { new Flurl.Url("testScreen.com"), new Flurl.Url("testsScreen2.com") },
                Version = "TestVersion",
                Tags = new List<Data.F95.F95Tag> { Data.F95.F95Tag.SciFi, Data.F95.F95Tag.Adventure }

            },
            LaunchExePath = Path.Join(appPath, _folderName, "testGameName.exe"),
        };

        var expectedJson = JObject.FromObject(game, _wrapper.Serializer);

        var hut = new SaveLocalGameCommandHandler(_gameRepo, _fileRepo);
        await hut.Handle(new SaveLocalGameCommand(game), CancellationToken.None);

        File.Exists(filePath).Should().BeTrue();
        var res = await File.ReadAllTextAsync(filePath);
        res.Should().Be(expectedJson.ToString());
    }

    [Fact]
    public async Task OverridesJsonFileAndSavesGameToIt()
    {
        var appPath = Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location);
        var filePath = Path.Combine(appPath!, _folderName, SettingsConsts.GameDataFileName);

        Directory.CreateDirectory(Path.Join(appPath, _folderName));

        //create and make sure the test file exists with content
        using ( var file = File.CreateText(filePath) )
            await file.WriteAsync("test");

        using ( var file = File.OpenText(filePath) )
        {
            var currentData = await file.ReadToEndAsync();
            currentData.Should().Be("test");
        }

        var game = new LocalGame
        {
            FolderName = _folderName,
            RootFolder = new GameLibraryFolder { Path = appPath! },
            Description = "testDes",
            CustomSearchTerm = "testSearchTerm",
            Title = "testGameTitle",
            F95Game = new Data.F95.F95Game
            {
                Id = 1,
                Title = "should appear",
                CoverUrl = "test cover",
                Screens = new List<Flurl.Url> { new Flurl.Url("testScreen.com"), new Flurl.Url("testsScreen2.com") },
                Version = "TestVersion",
                Tags = new List<Data.F95.F95Tag> { Data.F95.F95Tag.SciFi, Data.F95.F95Tag.Adventure }
            },
            LaunchExePath = Path.Combine(appPath!, "testGameName", "testGameName.exe")
        };

        var expectedJson = JObject.FromObject(game, _wrapper.Serializer);

        var hut = new SaveLocalGameCommandHandler(_gameRepo, _fileRepo);
        await hut.Handle(new SaveLocalGameCommand(game), CancellationToken.None);

        File.Exists(filePath).Should().BeTrue();
        var res = await File.ReadAllTextAsync(filePath);
        res.Should().Be(expectedJson.ToString());
    }

    [Fact]
    public async Task CreatesGameFolders()
    {
        var appPath = Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location);

        var game = new LocalGame
        {
            FolderName = _folderName,
        };

        var hut = new SaveLocalGameCommandHandler(_gameRepo, _fileRepo);
        await hut.Handle(new SaveLocalGameCommand(game), CancellationToken.None);

        File.Delete(Path.Combine(appPath!, _folderName, SettingsConsts.GameDataFileName));

        var gameFolderPath = Path.Join(appPath, _folderName, SettingsConsts.GameFolderName);
        var archiveFolderPath = Path.Join(appPath, _folderName, SettingsConsts.ArchivesFolderName);
        var modsFolderPath = Path.Join(appPath, _folderName, SettingsConsts.ModsFolderName);
        var savesFolderPath = Path.Join(appPath, _folderName, SettingsConsts.SavesFolderName);

        Directory.Exists(gameFolderPath).Should().BeTrue();
        Directory.Exists(archiveFolderPath).Should().BeTrue();
        Directory.Exists(modsFolderPath).Should().BeTrue();
        Directory.Exists(savesFolderPath).Should().BeTrue();
    }

    public void Dispose()
    {
        var appPath = Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location);
        var path = Path.Join(appPath, _folderName);
        if ( Directory.Exists(path) )
            Directory.Delete(Path.Join(appPath, _folderName), true);
    }
}
