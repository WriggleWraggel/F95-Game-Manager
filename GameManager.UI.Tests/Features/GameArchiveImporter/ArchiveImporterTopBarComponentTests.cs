using GameManager.UI.Features.GameArchiveImporter.Components;

namespace GameManager.UI.Tests.Features.GameArchiveImporter;

/// <summary>
/// Tests for ArchiveImporterTopBarComponent.
/// Verifies the four display states: add import folders prompt (no folders
/// configured), no compressed files (all clear), scanning/loading, and file count
/// when archives are detected.
/// </summary>
public class ArchiveImporterTopBarComponentTests : BUnitTestBase
{
    private void SetupStates(
        bool folderExists = true,
        bool scanning = false,
        List<FileMap>? fileMaps = null)
    {
        var settings = folderExists
            ? DefaultSettings()
            : new AppSettings { ImportFolders = new List<ArchiveImportFolder>() };

        SetupState(new SettingsState { Settings = settings, Initialized = true });
        SetupState(new GameArchiveImporterState
        {
            Scanning = scanning,
            FileMaps = fileMaps ?? new List<FileMap>()
        });
    }

    [Fact]
    public void ShowsAddImportFoldersButtonWhenNoFoldersConfigured()
    {
        SetupStates(folderExists: false);

        var cut = RenderComponent<ArchiveImporterTopBarComponent>();

        cut.Markup.Should().Contain("Add Import Folders");
    }

    [Fact]
    public void ShowsNoCompressedFilesButtonWhenNoFilesDetected()
    {
        SetupStates(folderExists: true, scanning: false, fileMaps: new List<FileMap>());

        var cut = RenderComponent<ArchiveImporterTopBarComponent>();

        cut.Markup.Should().Contain("No Compressed Files");
    }

    [Fact]
    public void ShowsSearchingButtonWhenScanning()
    {
        SetupStates(folderExists: true, scanning: true);

        var cut = RenderComponent<ArchiveImporterTopBarComponent>();

        cut.Markup.Should().Contain("Searching");
    }

    [Fact]
    public void ShowsFileCountButtonWhenFilesDetected()
    {
        var fileMaps = new List<FileMap>
        {
            new() { FilePath = @"C:\Downloads\game1.zip" },
            new() { FilePath = @"C:\Downloads\game2.zip" }
        };
        SetupStates(folderExists: true, scanning: false, fileMaps: fileMaps);

        var cut = RenderComponent<ArchiveImporterTopBarComponent>();

        cut.Markup.Should().Contain("2 Files Found");
    }
}
