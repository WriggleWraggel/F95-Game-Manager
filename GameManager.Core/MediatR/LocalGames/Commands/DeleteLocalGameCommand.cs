namespace GameManager.Core.MediatR.LocalGames.Commands;

public record DeleteLocalGameCommand(LocalGame Game) : IRequest;

internal class DeleteLocalGameCommandHandler : IRequestHandler<DeleteLocalGameCommand>
{
    private readonly IFileRepo _fileRepo;
    private readonly ILogger<DeleteLocalGameCommandHandler> _logger;

    public DeleteLocalGameCommandHandler(IFileRepo fileRepo, ILogger<DeleteLocalGameCommandHandler> logger)
    {
        _fileRepo = fileRepo;
        _logger = logger;
    }

    public Task Handle(DeleteLocalGameCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _fileRepo.DeleteDirectory(request.Game.FullPath);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "Error deleting game folder {GameFolder}: {Exception}", 
                request.Game.FullPath, ex.Message
            );
        }

        return Task.CompletedTask;
    }
}