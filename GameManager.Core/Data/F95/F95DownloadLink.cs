
namespace GameManager.Core.Data.F95;

public class F95DownloadLink
{
    [JsonIgnore]
    public F95Game Game { get; init; } = new F95Game();
    public string DownloadDescriptor { get; set; } = string.Empty;
    public Url DownloadUrl { get; set; } = new();

    [JsonIgnore]
    public bool F95Masked => DownloadUrl.ToString().Contains("f95zone.to/masked", StringComparison.OrdinalIgnoreCase);
    public DownloadTarget Target => DownloadUrl switch
    {
        { Host: var host } when host.Contains("mega") => DownloadTarget.Mega,
        { Host: var host } when host.Contains("anonfiles") => DownloadTarget.AnonFile,
        _ => DownloadTarget.Unsupported
    };

}

public enum DownloadTarget
{
    Mega,
    AnonFile,
    Unsupported
}

