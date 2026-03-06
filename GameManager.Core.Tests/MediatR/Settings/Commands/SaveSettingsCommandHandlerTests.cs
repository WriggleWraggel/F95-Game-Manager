using System.IO;
using System.Reflection;

using GameManager.Core.MediatR.Settings.Commands;

using Newtonsoft.Json;

namespace GameManager.Core.Tests.MediatR.Settings.Commands;

public class SaveSettingsCommandHandlerTests : IDisposable
{
    private readonly string _testPath;

    public SaveSettingsCommandHandlerTests()
    {
        _testPath = Path.Combine(Path.GetTempPath(), "F95GameManagerTests", Guid.NewGuid().ToString());
        Directory.CreateDirectory(_testPath);
    }

    [Fact]
    public async Task CreatesJsonFileAndSavesSettingsToIt()
    {
        var filePath = Path.Combine(_testPath, SettingsConsts.SettingsFileName);
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

        var hut = new SaveSettingsCommandHandler(new NewtonSoftJsonSerializerWrapper(), settingsSub);
        await hut.Handle(new SaveSettingsCommand
        { AppSettings = appSettings }, CancellationToken.None);

        File.Exists(filePath).Should().BeTrue();
        var res = await File.ReadAllTextAsync(filePath);
        res.Should().Be(JsonConvert.SerializeObject(appSettings, Formatting.Indented));
    }

    [Fact]
    public async Task OverridesJsonFileAndSavesSettingsToIt()
    {
        var filePath = Path.Combine(_testPath, SettingsConsts.SettingsFileName);

        using ( var file = File.CreateText(filePath) )
            await file.WriteAsync("test");

        using ( var file = File.OpenText(filePath) )
        {
            var currentData = await file.ReadToEndAsync();
            currentData.Should().Be("test");
        }

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

        var hut = new SaveSettingsCommandHandler(new NewtonSoftJsonSerializerWrapper(), settingsSub);
        await hut.Handle(new SaveSettingsCommand
        { AppSettings = appSettings }, CancellationToken.None);

        File.Exists(filePath).Should().BeTrue();
        var res = await File.ReadAllTextAsync(filePath);
        res.Should().Be(JsonConvert.SerializeObject(appSettings, Formatting.Indented));
    }

    public void Dispose()
    {
        if (Directory.Exists(_testPath))
            Directory.Delete(_testPath, true);
    }
}
