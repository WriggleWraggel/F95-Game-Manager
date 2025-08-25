using System.Diagnostics;

using GameManager.Core.Data.Settings;

namespace GameManager.Core.MediatR.ProcessLauncher.Commands;

public record LaunchGameCommand(LocalGame Game) : IRequest<Process?>;

internal class LaunchGameCommandHandler : IRequestHandler<LaunchGameCommand, Process?>
{
    private readonly IMediator _mediator;

    public LaunchGameCommandHandler(IMediator mediator) => _mediator = mediator;

    public async Task<Process?> Handle(LaunchGameCommand request, CancellationToken cancellationToken)
    {
        var gameLaunchPath = Path.Join(
            request.Game.FullPath,
            SettingsConsts.GameFolderName,
            request.Game.LaunchExePath);

        if ( request.Game.LaunchExePath.EndsWith("exe") )
        {
            return await _mediator.Send(new LaunchProccessCommand(new ProcessStartInfo
            {
                FileName = gameLaunchPath,
                WorkingDirectory = Directory.GetParent(gameLaunchPath)?.FullName,
            }));
        }
        else
        {
            return await _mediator.Send(new LaunchProccessCommand(new ProcessStartInfo
            {
                FileName = gameLaunchPath,
                UseShellExecute = true,
            }));
        }
    }
}
