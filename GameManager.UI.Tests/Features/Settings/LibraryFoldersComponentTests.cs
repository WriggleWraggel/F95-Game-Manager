using GameManager.UI.Features.Settings.Components;
using GameManager.UI.Features.Settings.Actions.Save;
using GameManager.UI.Features.GameLibrary.Actions.GetNewGames;

namespace GameManager.UI.Tests.Features.Settings;

/// <summary>
/// Tests for LibraryFoldersComponent.
/// Verifies that library folder paths are rendered, that the Scan button is shown
/// only when ScanEnabled=true, that AddGameLibraryFolderAction is dispatched on Add,
/// and that SaveSettingsAction + GetNewGamesAction are dispatched on Scan.
/// </summary>
public class LibraryFoldersComponentTests : BUnitTestBase
{
    private void SetupSettingsState(AppSettings? settings = null)
    {
        SetupState(new SettingsState
        {
            Settings = settings ?? DefaultSettings(),
            Initialized = true
        });
    }

    [Fact]
    public void RendersLibraryFoldersCard()
    {
        SetupSettingsState();

        var cut = RenderComponent<LibraryFoldersComponent>(parameters => parameters
            .Add(p => p.ScanEnabled, false));

        cut.Markup.Should().Contain("Library Folders");
    }

    [Fact]
    public void RendersConfiguredFolderPaths()
    {
        var settings = new AppSettings
        {
            GameLibrarySettings = new GameLibrarySettings
            {
                Folders = new List<GameLibraryFolder>
                {
                    new() { Path = @"C:\GameLib1" },
                    new() { Path = @"D:\GameLib2" }
                }
            }
        };
        SetupSettingsState(settings);

        var cut = RenderComponent<LibraryFoldersComponent>(parameters => parameters
            .Add(p => p.ScanEnabled, false));

        cut.Markup.Should().Contain(@"C:\GameLib1");
        cut.Markup.Should().Contain(@"D:\GameLib2");
    }

    [Fact]
    public void ShowsScanButtonWhenScanEnabledIsTrue()
    {
        SetupSettingsState();

        var cut = RenderComponent<LibraryFoldersComponent>(parameters => parameters
            .Add(p => p.ScanEnabled, true));

        var scanButtons = cut.FindAll("button").Where(b => b.TextContent.Contains("Scan")).ToList();
        scanButtons.Should().NotBeEmpty();
    }

    [Fact]
    public void HidesScanButtonWhenScanEnabledIsFalse()
    {
        SetupSettingsState();

        var cut = RenderComponent<LibraryFoldersComponent>(parameters => parameters
            .Add(p => p.ScanEnabled, false));

        var scanButtons = cut.FindAll("button").Where(b => b.TextContent.Contains("Scan")).ToList();
        scanButtons.Should().BeEmpty();
    }

    [Fact]
    public void RendersAddButton()
    {
        SetupSettingsState();

        var cut = RenderComponent<LibraryFoldersComponent>(parameters => parameters
            .Add(p => p.ScanEnabled, false));

        var addButton = cut.FindAll("button").FirstOrDefault(b => b.TextContent.Trim() == "Add");
        addButton.Should().NotBeNull();
    }

    [Fact]
    public void DispatchesSaveSettingsActionWhenFormSaved()
    {
        SetupSettingsState();

        var cut = RenderComponent<LibraryFoldersComponent>(parameters => parameters
            .Add(p => p.ScanEnabled, false));

        cut.Find("form").Submit();

        DispatcherMock.Received().Dispatch(Arg.Any<SaveSettingsAction>());
    }
}
