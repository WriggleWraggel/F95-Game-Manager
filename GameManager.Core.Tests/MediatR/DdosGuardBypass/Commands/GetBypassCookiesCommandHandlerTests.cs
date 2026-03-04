using GameManager.Core.MediatR.DdosGaurdBypass.Commands;

using Microsoft.Extensions.Logging;

namespace GameManager.Core.Tests.MediatR.F95.Commands;

public class GetBypassCookiesCommandHandlerTests
{
    [IntegrationFact]
    public async Task BypassSuccessful()
    {
        var wrapper = new HttpSessionWrapper();
        var logger = Substitute.For<ILogger<GetBypassCookiesCommandHandler>>();
        var hut = new GetBypassCookiesCommandHandler(wrapper, logger);
        await hut.Handle( new GetBypassCookiesCommand(new List<Flurl.Http.FlurlCookie>()), CancellationToken.None);
        wrapper.Session.Cookies.Should().Contain(_ => _.Name == "__ddgid_");
        wrapper.Session.Cookies.Should().Contain(_ => _.Name == "__ddgmark_");
    }
}
