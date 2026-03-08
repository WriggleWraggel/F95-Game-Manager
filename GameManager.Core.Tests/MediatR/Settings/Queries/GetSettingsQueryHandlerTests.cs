using System.IO;
using System.Reflection;

using GameManager.Core.MediatR.Settings.Commands;
using GameManager.Core.MediatR.Settings.Queries;

using Newtonsoft.Json;

namespace GameManager.Core.Tests.MediatR.Settings.Queries;

public class GetSettingsQueryHandlerTests : IDisposable
{
    private readonly string _testPath;

    public GetSettingsQueryHandlerTests()
    {
        _testPath = Path.Combine(Path.GetTempPath(), "F95GameManagerTests", Guid.NewGuid().ToString());
        Directory.CreateDirectory(_testPath);
    }

    [Fact]
    public async Task GetsSettingsFromJsonFile()
    {
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
                    new GameLibraryFolder{ Path = Path.Combine(Path.GetTempPath(), "Test") }
                }
            }
        };

        var settingsSub = Substitute.For<ISettingsPathProvider>();
        settingsSub.Path.Returns(_testPath);

        var createSettingsHandler = new SaveSettingsCommandHandler(new NewtonSoftJsonSerializerWrapper(), settingsSub);
        await createSettingsHandler.Handle(new SaveSettingsCommand
        { AppSettings = appSettings }, CancellationToken.None);

        var hut = new GetSettingsQueryHandler(wrapper, settingsSub);
        var res = await hut.Handle(new GetSettingsQuery(), CancellationToken.None);

        var expectedJson = JsonConvert.SerializeObject(appSettings, Formatting.Indented);
        var actualJson = JsonConvert.SerializeObject(res, Formatting.Indented);
        actualJson.Should().Be(expectedJson);
    }

    public void Dispose()
    {
        if (Directory.Exists(_testPath))
            Directory.Delete(_testPath, true);
    }
}
