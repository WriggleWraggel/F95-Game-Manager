using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GameManager.Core.Data.Settings;

namespace GameManager.Core.MediatR.LocalGames.Commands
{
    public record PurgeOldDataInGameFolderCommand(LocalGame Game, string NewGamePath) : IRequest;

    internal class PurgeOldDataInGameFolderCommandHandler : IRequestHandler<PurgeOldDataInGameFolderCommand>
    {
        public Task Handle(PurgeOldDataInGameFolderCommand request, CancellationToken cancellationToken)
        {

            var saveDirectories = Directory
                .EnumerateDirectories(request.Game.FullPath, "*", SearchOption.AllDirectories)
                .Where(_ => _ != request.NewGamePath && _.Contains("save", StringComparison.CurrentCultureIgnoreCase))
                .ToList();

            var filesToKeep = new List<string>
            {
                Path.GetFileName(request.Game.ArchiveFile),
                SettingsConsts.GameDataFileName
            };

            foreach ( var saveDir in saveDirectories )
            {
                var directoryName = Path.GetFileName(saveDir);
                Directory.Move(saveDir, Path.Join(request.NewGamePath, directoryName));
            }

            var directoriesToDelete = Directory
                .EnumerateDirectories(request.Game.FullPath, "*")
                .Where(_ => _ != request.NewGamePath && !_.Contains("save", StringComparison.CurrentCultureIgnoreCase));

            foreach ( var directory in directoriesToDelete )
            {
                Directory.Delete(directory, true);
            }

            var filesToDelete = Directory
                .EnumerateFiles(request.Game.FullPath, "*", SearchOption.AllDirectories)
                .Where(deleteFile => !filesToKeep.Contains(Path.GetFileName(deleteFile)) &&
                   !Path.GetDirectoryName(deleteFile)!.Contains(request.NewGamePath));

            foreach ( var file in filesToDelete )
            {
                File.Delete(file);
            }

            return Task.CompletedTask;
        }
    }
}
