using System.Diagnostics;

namespace GameManager.Core.MediatR.ProcessLauncher.Commands;

public record OpenFolderCommand(LocalGame Game) : IRequest<Process?>;

internal class OpenFolderCommandHandler : IRequestHandler<OpenFolderCommand, Process?>
{
    private readonly IMediator _mediator;

    public OpenFolderCommandHandler(IMediator mediator) => _mediator = mediator;

    public async Task<Process?> Handle(OpenFolderCommand request, CancellationToken cancellationToken) =>
        await _mediator.Send(new LaunchProccessCommand(new ProcessStartInfo
        {
            FileName = request.Game.FullPath,
            UseShellExecute = true,
        }));
}
