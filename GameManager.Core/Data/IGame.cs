namespace GameManager.Core.Data;

public interface IGame
{
    public string Title { get; }
    public Url GameUrl { get; }
    public Url CoverUrl { get; }
    public string Version { get; }
    public GameEngine GameEngine { get; }
}

public enum GameEngine
{
    Renpy,
    Unity,
    Html,
    Rpgm,
    Flash,
    Java,
    Qsp,
    Rags,
    Unreal,
    WolfRpg,
    Unknown
}