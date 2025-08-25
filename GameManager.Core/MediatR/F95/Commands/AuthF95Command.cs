using Microsoft.Extensions.Logging;

namespace GameManager.Core.MediatR.F95.Commands;

public class AuthF95Command : IRequest<AuthResult>
{
    public string Username { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public List<FlurlCookie> Cookies { get; set; } = new();
}

public class AuthResult
{
    public bool Success { get; set; }
    public Exception? FailureEx { get; set; }
    public List<FlurlCookie> Cookies { get; set; } = new();
}

internal class AuthF95CommandHandler : IRequestHandler<AuthF95Command, AuthResult>
{
    private readonly HttpSessionWrapper _wrapper;
    private readonly ILogger<AuthF95CommandHandler> _logger;
    public AuthF95CommandHandler(HttpSessionWrapper wrapper, ILogger<AuthF95CommandHandler> logger)
    {
        _wrapper = wrapper;
        _logger = logger;
    }

    public async Task<AuthResult> Handle(AuthF95Command request, CancellationToken cancellationToken)
    {
        try
        {
            if ( request.Cookies.Any() && !_wrapper.Session.Cookies.Any() )
                foreach ( var cookie in request.Cookies )
                {
                    var success = _wrapper.Session.Cookies.TryAddOrReplace(cookie, out var cookieFailedReason);
                    if ( !success )
                        _logger.LogInformation($"error adding stored cookie {cookie.Name} because ${cookieFailedReason}");
                }

            if ( CheckIsLoggedIn() )
                return new AuthResult
                {
                    Success = true,
                    Cookies = _wrapper.Session.Cookies.ToList(),
                };

            var loginHtmlPage = await GetLoginPage(cancellationToken);

            var loginPageToken = loginHtmlPage.DocumentNode.Descendants("input")
                .Where(i => i.Attributes.Where(a => a.Value == "_xfToken").Any())
                .FirstOrDefault()?.Attributes
                .Where(a => a.Name == "value")
                .FirstOrDefault()?.Value ?? string.Empty;

            var formContents = new KeyValuePair<string, string>[]
            {
                new ("login", request.Username),
                new ("url", ""),
                new ("password", request.Password),
                new ("password_confirm", ""),
                new ("additional_security", ""),
                new ("remember", "1"),
                new ("_xfRedirect", "/"),
                new ("website_code", ""),
                new ("_xfToken", loginPageToken)
            };

            var loginResult = await _wrapper.Session
                .Request(HttpConsts.F95Login)
                .WithHeader("Sec-Fetch-Dest", "document")
                .WithHeader("Sec-Fetch-Mode", "navigate")
                .WithHeader("Sec-Fetch-Site", "none")
                .WithHeader("Sec-Fetch-User", "?1")
                .WithHeader("TE", "trailers")
                .WithHeader("DNT", "1")
                .PostUrlEncodedAsync(formContents);

            return new AuthResult
            {
                Success = _wrapper.Session.Cookies.Any(_ => _.Name == "xf_user"),
                Cookies = _wrapper.Session.Cookies.ToList(),
            };
        }
        catch ( Exception ex )
        {
            _logger.LogError(ex, "Error logging into f95");
            return new AuthResult
            {
                Success = false,
                FailureEx = ex
            };
        }
    }

    private async Task<HtmlDocument> GetLoginPage(CancellationToken cancellationToken)
    {
        var LoginPageContent = await _wrapper.Session
            .Request(HttpConsts.F95Login)
            .WithHeader("Sec-Fetch-Dest", "document")
            .WithHeader("Sec-Fetch-Mode", "navigate")
            .WithHeader("Sec-Fetch-Site", "none")
            .WithHeader("Sec-Fetch-User", "?1")
            .WithHeader("TE", "trailers")
            .WithHeader("DNT", "1")
            .GetStringAsync(cancellationToken: cancellationToken);

        var loginHtmlPage = new HtmlDocument();
        loginHtmlPage.LoadHtml(LoginPageContent);
        return loginHtmlPage;
    }

    private bool CheckIsLoggedIn()
    {
        return _wrapper.Session.Cookies.FirstOrDefault(_ => _.Name == "xf_user")?.Expires > DateTime.UtcNow;
    }
}

