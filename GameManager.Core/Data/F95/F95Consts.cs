namespace GameManager.Core.Data.F95;

public static class F95Consts
{
    public readonly static F95GamePrefix[] F95GameEnginePrefixes = new[]
    {
        F95GamePrefix.Adrift,
        F95GamePrefix.Flash,
        F95GamePrefix.Html,
        F95GamePrefix.Java,
        F95GamePrefix.Others,
        F95GamePrefix.Qsp,
        F95GamePrefix.Rags,
        F95GamePrefix.Rpgm,
        F95GamePrefix.Renpy,
        F95GamePrefix.Tads,
        F95GamePrefix.Unity,
        F95GamePrefix.UnrealEngine,
        F95GamePrefix.WebGl,
        F95GamePrefix.WolfRpg,
    };

    public readonly static F95GamePrefix[] OtherPrefixes = new[]
    {
        F95GamePrefix.Vn,
        F95GamePrefix.Siterip,
        F95GamePrefix.Collection
    };

    public readonly static F95GamePrefix[] StatusPrefixes = new[]
    {
        F95GamePrefix.Abandoned,
        F95GamePrefix.Completed,
        F95GamePrefix.OnHold
    };

    public readonly static Dictionary<F95GamePrefix, GameEngine> GameEngineMap = new()
    {
        { F95GamePrefix.Adrift, GameEngine.Unknown },
        { F95GamePrefix.Flash, GameEngine.Flash },
        { F95GamePrefix.Html, GameEngine.Html },
        { F95GamePrefix.Java, GameEngine.Java },
        { F95GamePrefix.Others, GameEngine.Unknown },
        { F95GamePrefix.Qsp, GameEngine.Qsp },
        { F95GamePrefix.Rags, GameEngine.Rags },
        { F95GamePrefix.Rpgm, GameEngine.Rpgm },
        { F95GamePrefix.Renpy, GameEngine.Renpy },
        { F95GamePrefix.Tads, GameEngine.Unknown },
        { F95GamePrefix.Unity, GameEngine.Unity },
        { F95GamePrefix.UnrealEngine, GameEngine.Unreal },
        { F95GamePrefix.WebGl, GameEngine.Unknown },
        { F95GamePrefix.WolfRpg, GameEngine.WolfRpg }
    };
}
