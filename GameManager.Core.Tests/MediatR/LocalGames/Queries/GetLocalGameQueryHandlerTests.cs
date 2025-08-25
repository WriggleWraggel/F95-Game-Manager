using System.IO;
using System.Reflection;

using GameManager.Core.MediatR.LocalGames.Commands;
using GameManager.Core.MediatR.LocalGames.Queries;

using Newtonsoft.Json;

namespace GameManager.Core.Tests.MediatR.LocalGames.Queries;

public class GetLocalGameQueryHandlerTests
{

    private NewtonSoftJsonSerializerWrapper _wrapper;
    private IFileRepo _fileRepo;
    private ILocalGameRepo _gameRepo;

    public GetLocalGameQueryHandlerTests()
    {
        _wrapper = new();
        _fileRepo = new FileRepo();
        _gameRepo = new LocalGameJsonFileRepo(_wrapper);
    }

    [Fact]
    public async Task GetsGameDataFromJsonFile()
    {
        var appPath = Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location);
        var filePath = Path.Combine(appPath!, "testGameName", "gameData.json");
        var wrapper = new NewtonSoftJsonSerializerWrapper();

        var game = new LocalGame
        {
            FolderName = "testGameName",
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

        var fileCreateHandler = new SaveLocalGameCommandHandler(_gameRepo, _fileRepo);
        await fileCreateHandler.Handle(new SaveLocalGameCommand(game), CancellationToken.None);

        var hut = new GetLocalGameQueryHandler(wrapper);
        var res = await hut.Handle(new GetLocalGameQuery { GameFolderPath = Path.Join(appPath!, "testGameName") }, CancellationToken.None);

        File.Delete(filePath);

        var expectedJson = JsonConvert.SerializeObject(game, Formatting.Indented);
        var actualJson = JsonConvert.SerializeObject(res, Formatting.Indented);
        actualJson.Should().Be(expectedJson);
    }
}
