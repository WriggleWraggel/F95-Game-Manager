using GameManager.Core;
using GameManager.Core.Services;
using GameManager.UI;

using Microsoft.Extensions.Logging;

namespace F95GameManager;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var settingsPathProvider = new AppDataSettingsPathProvider();
        var flushInterval = new TimeSpan(0, 0, 1);
        var logFilePath = Path.Combine(settingsPathProvider.Path, "GameManager.log");

        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        builder.Services.AddMauiBlazorWebView();
        builder.Services.AddBlazorWebViewDeveloperTools();

        builder.Services.AddUI();

        builder.Services.AddCore(settingsPathProvider);

        return builder.Build();
    }


    public class AppDataSettingsPathProvider : ISettingsPathProvider
    {
        public string Path => FileSystem.AppDataDirectory;
    }
}