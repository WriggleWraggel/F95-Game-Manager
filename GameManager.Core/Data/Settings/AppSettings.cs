namespace GameManager.Core.Data.Settings;

public class AppSettings
{
    public F95AuthSettings AuthSettings { get; set; } = new();
    public F95SearchSettings SearchSettings { get; set; } = new();
    public GameLibrarySettings GameLibrarySettings { get; set; } = new();
    public List<ArchiveImportFolder> ImportFolders { get; set; } = new();
    public List<string> AppliedMigrations { get; set; } = new();
    public string SevenZipPath { get; set; } = "";
}