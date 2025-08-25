using GameManager.Core.MediatR.DdosGaurdBypass.Commands;
using GameManager.Core.MediatR.F95.Commands;

using Microsoft.Extensions.Logging;

namespace GameManager.Core.Tests.MediatR.F95.Commands;

public class AuthF95CommandHandlerTests
{
    [Fact(Skip = "sensitive data")]
    public async Task AuthSucceedWithCorrectUsernameAndPassword()
    {
        var wrapper = new HttpSessionWrapper();
        var bypassCommand = new GetBypassCookiesCommandHandler(wrapper, Substitute.For<ILogger<GetBypassCookiesCommandHandler>>());
        await bypassCommand.Handle(new GetBypassCookiesCommand(new List<Flurl.Http.FlurlCookie>()), CancellationToken.None);

        var hut = new AuthF95CommandHandler(wrapper, Substitute.For<ILogger<AuthF95CommandHandler>>());
        var res = await hut.Handle(new AuthF95Command { Username = "", Password = "" }, CancellationToken.None);
        wrapper.Session.Cookies.Should().Contain(_ => _.Name == "xf_session");
        wrapper.Session.Cookies.Should().Contain(_ => _.Name == "xf_user");
        res.Success.Should().BeTrue();
    }
}