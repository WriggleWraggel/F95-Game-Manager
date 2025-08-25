
using System.Net;

using Microsoft.Extensions.Logging;

namespace GameManager.Core.MediatR.DdosGaurdBypass.Commands;

public record GetBypassCookiesCommand(List<FlurlCookie> Cookies) : IRequest;

internal class GetBypassCookiesCommandHandler : IRequestHandler<GetBypassCookiesCommand>
{
    private readonly HttpSessionWrapper _wrapper;
    private readonly ILogger<GetBypassCookiesCommandHandler> _logger;
    public GetBypassCookiesCommandHandler(HttpSessionWrapper wrapper, ILogger<GetBypassCookiesCommandHandler> logger)
    {
        _wrapper = wrapper;
        _logger = logger;
    }

    public async Task Handle(GetBypassCookiesCommand request, CancellationToken cancellationToken)
    {
        try
        {
            foreach ( var cookie in request.Cookies ?? new List<FlurlCookie>() )
            {
                _wrapper.Session.Cookies.TryAddOrReplace(cookie, out var failReason);
            }

            //let the session get populated with the base cookies we need from f95
            var f95initPokeRes = await _wrapper.Session
                .Request(HttpConsts.F95Login)
                .WithTimeout(10)
                .AllowHttpStatus(HttpStatusCode.Forbidden.ToString())
                .WithHeader("Accept", "text/html")
                .WithHeader("Accept-Language", "en-US")
                .WithHeader("Sec-Fetch-Dest", "document")
                .WithHeader("Sec-Fetch-Mode", "navigate")
                .WithHeader("Sec-Fetch-Site", "none")
                .WithHeader("Sec-Fetch-User", "?1")
                .WithHeader("TE", "trailers")
                .WithHeader("DNT", "1")
                .GetAsync(cancellationToken: cancellationToken);

            var ddosGuardRes = await _wrapper.Session
                .Request(HttpConsts.DdosGaurdCheckJs)
                .WithTimeout(10)
                .WithHeader("Accept", "*/*")
                .WithHeader("Accept-Language", "en-US,en;q=0.5")
                .WithHeader("Accept-Encoding", "gzip, deflate")
                .WithHeader("Referer", HttpConsts.F95Root)
                .WithHeader("Sec-Fetch-Dest", "script")
                .WithHeader("Sec-Fetch-Mode", "no-cors")
                .WithHeader("Sec-Fetch-Site", "cross-site")
                .GetStringAsync(cancellationToken: cancellationToken);

            var ddosGuardId = ddosGuardRes
                .Split("'/.well-known/ddos-guard/id/")[1]
                .Split("'")[0];

            var ddosGaurdF95Request = await _wrapper.Session
                .Request(HttpConsts.F95Root, HttpConsts.DdosGuardF95Handshake, ddosGuardId)
                .WithTimeout(10)
                .WithHeader("Referer", HttpConsts.F95Root)
                .WithHeader("Accept", "image/webp,*/*")
                .WithHeader("Accept-Language", "en-US,en;q=0.5")
                .WithHeader("Accept-Encoding", "gzip, deflate")
                .WithHeader("Cache-Control", "no-cache")
                .WithHeader("Sec-Fetch-Dest", "script")
                .WithHeader("Sec-Fetch-Mode", "no-cors")
                .WithHeader("Sec-Fetch-Site", "cross-site")
                .GetAsync(cancellationToken: cancellationToken);
        }
        catch ( Exception e )
        {
            _logger.LogError(e, "ddod gaurd bypass cookies");
            throw;
        }
    }

}

