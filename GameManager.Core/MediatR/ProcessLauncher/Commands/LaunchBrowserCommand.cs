using System.Diagnostics;

namespace GameManager.Core.MediatR.ProcessLauncher.Commands;

public record LaunchBrowserCommand(string url) : IRequest<Process?>;

internal class LaunchBrowserCommandHandler : IRequestHandler<LaunchBrowserCommand, Process?>
{
    private readonly IMediator _mediator;

    public LaunchBrowserCommandHandler(IMediator mediator) => _mediator = mediator;

    public async Task<Process?> Handle(LaunchBrowserCommand request, CancellationToken cancellationToken) =>
        await _mediator.Send(new LaunchProccessCommand(new ProcessStartInfo
        {
            FileName = request.url,
            UseShellExecute = true,
        }));
}
