using GameManager.Core.Data.Settings;

namespace GameManager.Core.Data.F95;

public class F95Game : IGame
{
    public F95Game() { }

    public F95Game(F95Game f95Game)
    {
        Id = f95Game.Id;
        Title = f95Game.Title;
        Version = f95Game.Version;
        Tags = f95Game.Tags;
        CoverUrl = f95Game.CoverUrl;
        Screens = f95Game.Screens;
        Creator = f95Game.Creator;
        Likes = f95Game.Likes;
        Rating = f95Game.Rating;
        ThreadLastUpdatedDate = f95Game.ThreadLastUpdatedDate;
        DownloadLinks = f95Game.DownloadLinks;
    }

    [JsonProperty("cover")]
    public Url CoverUrl { get; set; } = new();

    public string Creator { get; set; } = string.Empty;

    public List<F95DownloadLink> DownloadLinks { get; set; } = new();

    public Url GameUrl => new Url($"https://f95zone.to/threads/{Id}");

    [JsonProperty("thread_id")]
    public int Id { get; set; }

    public DateTime? ThreadLastUpdatedDate { get; set; } = null;
    public double Likes { get; set; } = 0;
    public List<F95GamePrefix> Prefixes { get; set; } = new();
    public double Rating { get; set; } = 0;
    public List<Url> Screens { get; set; } = new();
    public List<F95Tag> Tags { get; set; } = new();
    public string Title { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;

    public GameEngine GameEngine
    {
        get
        {
            var matchedEngines = F95Consts.F95GameEnginePrefixes.Intersect(Prefixes);
            if ( !matchedEngines.Any() )
                return GameEngine.Unknown;

            return F95Consts.GameEngineMap[matchedEngines.First()];
        }
    }
}