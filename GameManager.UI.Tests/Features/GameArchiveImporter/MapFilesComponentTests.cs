using GameManager.UI.Features.GameArchiveImporter.Components;

namespace GameManager.UI.Tests.Features.GameArchiveImporter;

/// <summary>
/// Tests for MapFilesComponent.
/// Verifies the scanning state, the no-files message, and that file map entries
/// are rendered when detected archives are present.
/// </summary>
public class MapFilesComponentTests : BUnitTestBase
{
    private void SetupStates(
        bool scanning = false,
        List<FileMap>? fileMaps = null)
    {
        SetupState(new GameArchiveImporterState
        {
            Scanning = scanning,
            FileMaps = fileMaps ?? new List<FileMap>()
        });
        SetupState(new GameLibraryState { Games = new List<LocalGame>() });
        SetupState(new OnlineSearchState());
    }

    [Fact]
    public void ShowsScanningWhenScanning()
    {
        SetupStates(scanning: true);

        var cut = RenderComponent<MapFilesComponent>();

        cut.Markup.Should().Contain("Scanning");
    }

    [Fact]
    public void ShowsNoCompressedFilesFoundWhenNoFileMaps()
    {
        SetupStates(scanning: false, fileMaps: new List<FileMap>());

        var cut = RenderComponent<MapFilesComponent>();

        cut.Markup.Should().Contain("No compressed files found");
    }

    [Fact]
    public void RendersFileMapEntriesWhenFilesExist()
    {
        var fileMaps = new List<FileMap>
        {
            new() { FilePath = @"C:\Downloads\adventure_v1.zip", Processing = false },
            new() { FilePath = @"C:\Downloads\rpg_v2.zip", Processing = false }
        };
        SetupStates(scanning: false, fileMaps: fileMaps);

        var cut = RenderComponent<MapFilesComponent>();

        // Both file paths should appear (FileMapComponent renders the filename)
        cut.Markup.Should().Contain("adventure_v1.zip");
        cut.Markup.Should().Contain("rpg_v2.zip");
    }
}
