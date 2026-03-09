using GameManager.UI.Layout;

using Microsoft.AspNetCore.Components.WebView.WindowsForms;
using Microsoft.Extensions.DependencyInjection;

namespace F95GameManager.WinForms;

public class MainForm : Form
{
    public MainForm(IServiceProvider serviceProvider)
    {
        Text = "F95 Game Manager";
        Width = 1280;
        Height = 800;
        WindowState = FormWindowState.Maximized;

        var blazorWebView = new BlazorWebView
        {
            Dock = DockStyle.Fill,
            HostPage = @"wwwroot\index.html",
            Services = serviceProvider,
        };

        blazorWebView.RootComponents.Add<Components.Routes>("#app");

        Controls.Add(blazorWebView);
    }
}
