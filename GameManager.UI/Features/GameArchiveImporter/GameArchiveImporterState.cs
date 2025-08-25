namespace GameManager.UI.Features.GameArchiveImporter;

[FeatureState]
public record GameArchiveImporterState
{
    public bool Scanning { get; init; } = false;
    public List<FileMap> FileMaps { get; init; } = new();
}

public class FileMap
{
    public string FilePath { get; set; } = "";
    public Guid? GameId { get; set; }
    public bool Processing { get; set; } = false;
    public string Version { get; set; } = "";
    public bool RemoveOldFile { get; set; } = true;
}