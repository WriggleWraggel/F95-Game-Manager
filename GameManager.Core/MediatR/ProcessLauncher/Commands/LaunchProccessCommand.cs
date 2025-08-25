using System.Diagnostics;

namespace GameManager.Core.MediatR.ProcessLauncher.Commands;

public record LaunchProccessCommand(ProcessStartInfo ProcessInfo) : IRequest<Process?>;

internal class LaunchProccessCommandHandler : IRequestHandler<LaunchProccessCommand, Process?>
{
    public Task<Process?> Handle(LaunchProccessCommand request, CancellationToken cancellationToken) =>
        Task.FromResult(Process.Start(request.ProcessInfo));
}
