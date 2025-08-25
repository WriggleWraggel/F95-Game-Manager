using System.IO;
using System.Reflection;

using GameManager.Core.MediatR.Settings.Commands;
using GameManager.Core.MediatR.Settings.Queries;

using Newtonsoft.Json;

namespace GameManager.Core.Tests.MediatR.Settings.Queries;

public class GetSettingsQueryHandlerTests
{
    [Fact]
    public async Task GetsSettingsFromJsonFile()
    {
        var appPath = Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location);
        var filePath = Path.Combine(appPath!, "settings.json");
        var wrapper = new NewtonSoftJsonSerializerWrapper();

        var appSettings = new AppSettings
        {
            AuthSettings = new F95AuthSettings
            {
                Username = "test",
                Password = "testPw"
            },
            GameLibrarySettings = new()
            {
                Folders = new List<GameLibraryFolder>
                {
                    new GameLibraryFolder{ Path = @"D:\Test" }
                }
            }
        };

        var settingsSub = Substitute.For<ISettingsPathProvider>();
        settingsSub.Path.Returns(appPath);

        var createSettingsHandler = new SaveSettingsCommandHandler(new NewtonSoftJsonSerializerWrapper(), settingsSub);
        await createSettingsHandler.Handle(new SaveSettingsCommand
        { AppSettings = appSettings }, CancellationToken.None);

        var hut = new GetSettingsQueryHandler(wrapper, settingsSub);
        var res = await hut.Handle(new GetSettingsQuery(), CancellationToken.None);

        File.Delete(filePath);

        var expectedJson = JsonConvert.SerializeObject(appSettings, Formatting.Indented);
        var actualJson = JsonConvert.SerializeObject(res, Formatting.Indented);
        actualJson.Should().Be(expectedJson);
    }

}
