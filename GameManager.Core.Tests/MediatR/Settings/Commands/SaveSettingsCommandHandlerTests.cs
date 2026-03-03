using System.IO;
using System.Reflection;

using GameManager.Core.MediatR.Settings.Commands;

using Newtonsoft.Json;

namespace GameManager.Core.Tests.MediatR.Settings.Commands;

public class SaveSettingsCommandHandlerTests
{
    [Fact]
    public async Task CreatesJsonFileAndSavesSettingsToIt()
    {
        var appPath = Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location);
        var filePath = Path.Combine(appPath!, SettingsConsts.SettingsFileName);
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

        var hut = new SaveSettingsCommandHandler(new NewtonSoftJsonSerializerWrapper(), settingsSub);
        await hut.Handle(new SaveSettingsCommand
        { AppSettings = appSettings }, CancellationToken.None);

        File.Exists(filePath).Should().BeTrue();
        var res = await File.ReadAllTextAsync(filePath);
        res.Should().Be(JsonConvert.SerializeObject(appSettings, Formatting.Indented));
        File.Delete(filePath);
    }
    [Fact]
    public async Task OverridesJsonFileAndSavesSettingsToIt()
    {
        var appPath = Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location);
        var filePath = Path.Combine(appPath!, SettingsConsts.SettingsFileName);

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
                    new GameLibraryFolder{ Path = @"D:\Test" }
                }
            }
        };

        var settingsSub = Substitute.For<ISettingsPathProvider>();
        settingsSub.Path.Returns(appPath);

        var hut = new SaveSettingsCommandHandler(new NewtonSoftJsonSerializerWrapper(), settingsSub);
        await hut.Handle(new SaveSettingsCommand
        { AppSettings = appSettings }, CancellationToken.None);

        File.Exists(filePath).Should().BeTrue();
        var res = await File.ReadAllTextAsync(filePath);
        res.Should().Be(JsonConvert.SerializeObject(appSettings, Formatting.Indented));
        File.Delete(filePath);
    }
}
