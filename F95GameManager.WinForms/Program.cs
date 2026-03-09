using GameManager.Core;
using GameManager.Core.Services;
using GameManager.UI;

using Microsoft.AspNetCore.Components.WebView.WindowsForms;
using Microsoft.Extensions.DependencyInjection;

namespace F95GameManager.WinForms;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        var services = new ServiceCollection();
        services.AddWindowsFormsBlazorWebView();

        services.AddUI();

        var settingsPathProvider = new AppDataSettingsPathProvider();
        services.AddCore(settingsPathProvider);

        using var serviceProvider = services.BuildServiceProvider();

        var form = new MainForm(serviceProvider);
        Application.Run(form);
    }

    public class AppDataSettingsPathProvider : ISettingsPathProvider
    {
        public string Path => System.IO.Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "F95GameManager");
    }
}
