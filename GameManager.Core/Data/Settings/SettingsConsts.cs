namespace GameManager.Core.Data.Settings;
public static class SettingsConsts
{
    public const string GameDataFileName = "GameManager.GameData.json";
    public const string SettingsFileName = "GameManager.Settings.json";
    public static readonly string[] CompressedFilesTypeExtensions = new[]
    {
        ".zip",
        ".arj",
        ".deb",
        ".pkg",
        ".rar",
        ".rpm",
        ".tar.gz",
        ".z",
        ".zip",
        ".7z"
    };

    //TODO Add other os stuff to this
    public static readonly string[] GameExecutableFileExtensions = new[]
    {
        ".exe",
        ".html"
    };

    public static readonly string[] GameExecuteablePathExlcusions = new[]
    {
        "lib",
    };

    public const string ArchivesFolderName = "Archives";
    public const string ModsFolderName = "Mods";
    public const string SavesFolderName = "Saves";
    public const string GameFolderName = "Game";
}