namespace GameManager.Core.Data.Settings;

public class F95AuthSettings
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public List<FlurlCookie> Cookies { get; set; } = new();
}
