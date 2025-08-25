namespace GameManager.Core.Http;

internal class HttpSessionWrapper
{
    public HttpSessionWrapper()
    {
        Session = new CookieSession();
    }

    public CookieSession Session { get; }
    public void AddF95Cookies(Dictionary<string, string> cookies)
    {
        foreach ( var cookie in cookies )
        {
            Session.Cookies.AddOrReplace(cookie.Key, cookie.Value, "https://f95zone.to/");
        }
    }
}
