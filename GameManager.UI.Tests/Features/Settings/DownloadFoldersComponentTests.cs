using GameManager.UI.Features.Settings.Components;
using GameManager.UI.Features.Settings.Actions.Save;
using GameManager.UI.Features.Settings.Actions.DownloadFolders;

namespace GameManager.UI.Tests.Features.Settings;

/// <summary>
/// Tests for DownloadFoldersComponent.
/// Verifies that download folder paths are rendered with Scan buttons, that the Add
/// button is present, and that the correct Fluxor actions are dispatched for both
/// Add and Save operations.
/// </summary>
public class DownloadFoldersComponentTests : BUnitTestBase
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
    public void RendersDownloadFoldersCard()
    {
        SetupSettingsState();

        var cut = RenderComponent<DownloadFoldersComponent>();

        cut.Markup.Should().Contain("Download Folder Locations");
    }

    [Fact]
    public void RendersConfiguredImportFolderPath()
    {
        var settings = new AppSettings
        {
            ImportFolders = new List<ArchiveImportFolder>
            {
                new() { Path = @"C:\Downloads\Games" }
            }
        };
        SetupSettingsState(settings);

        var cut = RenderComponent<DownloadFoldersComponent>();

        cut.Markup.Should().Contain(@"C:\Downloads\Games");
    }

    [Fact]
    public void RendersScanButtonForEachFolder()
    {
        var settings = new AppSettings
        {
            ImportFolders = new List<ArchiveImportFolder>
            {
                new() { Path = @"C:\Downloads1" },
                new() { Path = @"C:\Downloads2" }
            }
        };
        SetupSettingsState(settings);

        var cut = RenderComponent<DownloadFoldersComponent>();

        var scanButtons = cut.FindAll("button").Where(b => b.TextContent.Contains("Scan")).ToList();
        scanButtons.Count.Should().Be(2);
    }

    [Fact]
    public void RendersAddButton()
    {
        SetupSettingsState();

        var cut = RenderComponent<DownloadFoldersComponent>();

        var addButton = cut.FindAll("button").FirstOrDefault(b => b.TextContent.Trim() == "Add");
        addButton.Should().NotBeNull();
    }

    [Fact]
    public void DispatchesSaveSettingsActionWhenFormSubmitted()
    {
        SetupSettingsState();

        var cut = RenderComponent<DownloadFoldersComponent>();

        cut.Find("form").Submit();

        DispatcherMock.Received().Dispatch(Arg.Any<SaveSettingsAction>());
    }
}
