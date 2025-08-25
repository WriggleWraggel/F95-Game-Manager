namespace GameManager.UI;

internal class Consts
{
    public const string VersionPattern1 = @"[a-zA-Z]*\d.*[a-zA-Z]*";
    public const string VersionPattern2 = @".+?(?=[^a-zA-Z0-9\.]+)";
    public static List<string> ProbablyNotTitle { get; } = new() { "v", "ver", "pc", "ep", "episode", "rev"};
}