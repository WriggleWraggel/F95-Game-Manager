using System.ComponentModel;

using GameManager.Core.Data.Settings;
using GameManager.Core.Extensions;

namespace GameManager.Core.Data;

public class LocalGame : IGame
{
    public LocalGame()
    {
    }

    public LocalGame(F95Game f95Game, GameLibraryFolder rootFolder)
    {
        F95Game = f95Game;
        FolderName = f95Game.Title;
        RootFolder = rootFolder;

        CoverUrl = f95Game.CoverUrl;
        ScreenUrls = f95Game.Screens;
        Title = f95Game.Title;
        Version = f95Game.Version;
        Creator = f95Game.Creator;
        GameEngine = f95Game.GameEngine;
        GameUrl = f95Game.GameUrl;
    }
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Root folder for the game
    /// </summary>
    public GameLibraryFolder RootFolder { get; set; } = new();

    [DisplayName("Folder Name")]
    public string? FolderName { set => _folderName = value?.MakeValidFileName(); get => _folderName; }
    private string? _folderName;


    [DisplayName("Full Path")]
    public string FullPath => Path.Combine(RootFolder.Path, _folderName ?? "");

    /// <summary>
    /// Relative path of exe
    /// </summary>
    [DisplayName("Launch Path")]
    public string LaunchExePath { get; set; } = string.Empty;

    [DisplayName("Last Played")]
    public DateTime? LastPlayed { get; set; }

    [DisplayName("Play Status")]
    public PlayStatus PlayStatus { get; set; } = PlayStatus.NotPlayed;

    [DisplayName("Times Played")]
    public int TiemsPlayed { get; set; } = 0;

    [DisplayName("Title")]
    public string Title { get; set; } = string.Empty;
    public string CustomSearchTerm { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Url GameUrl { get; set; } = new();
    public Url CoverUrl { get; set; } = new();
    public List<Url> ScreenUrls { get; set; } = new();
    public string Creator { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;

    [DisplayName("Date Added")]
    public DateTime DateAdded { get; set; } = DateTime.Now;

    public F95Game? F95Game { get; set; }
    public string ArchiveFile { get; set; } = string.Empty;

    [DisplayName("Archive File Last Updated Date")]
    public DateTime? ArchiveFileLastUpdated { get; set; }

    [DisplayName("Last Unziped Date")]
    public DateTime? ArchiveUnzipedDate { get; set; }

    [DisplayName("Update Available")]
    public bool UpdateAvailable { get; set; }

    [JsonIgnore]
    public bool Saved { get; set; } = false;

    public GameEngine GameEngine { get; set; } = GameEngine.Unknown;
}

public enum PlayStatus
{
    NotPlayed,
    Playing,
    Paused,
    Finished,
    Abandoned
}

