using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GameManager.Core.Data.Settings;

namespace GameManager.Core.MediatR.LocalGames.Commands
{
    public record DeleteOldLastDownloadFileCommandFileCommand(LocalGame Game) : IRequest;

    internal class DeleteOldLastDownloadFileCommandFileCommandHandler : IRequestHandler<DeleteOldLastDownloadFileCommandFileCommand>
    {
        private readonly IFileRepo _fileRepo;

        public DeleteOldLastDownloadFileCommandFileCommandHandler(IFileRepo fileRepo)
        {
            _fileRepo = fileRepo;
        }

        public Task Handle(DeleteOldLastDownloadFileCommandFileCommand request, CancellationToken cancellationToken)
        {
            _fileRepo.DeleteFile(Path.Join(request.Game.FullPath, SettingsConsts.ArchivesFolderName, request.Game.ArchiveFile));
            return Task.CompletedTask;
        }
    }
}
