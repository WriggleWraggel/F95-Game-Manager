using GameManager.Core.Data.Settings;

namespace GameManager.Core.MediatR.GameArchiveImporter.Queries;


public record GetAllCompressedFilesInFoldersQuery(List<ArchiveImportFolder> DownloadFolders) : IRequest<List<string>>;

internal class GetCompressedFilesInFoldersQueryHandler : IRequestHandler<GetAllCompressedFilesInFoldersQuery, List<string>>
{
    private readonly IMediator _mediator;

    public GetCompressedFilesInFoldersQueryHandler(IMediator mediator) => _mediator = mediator;

    public async Task<List<string>> Handle(GetAllCompressedFilesInFoldersQuery request, CancellationToken cancellationToken)
    {
        var tasks = new List<Task<List<string>>>();
        foreach ( var folder in request.DownloadFolders )
            tasks.Add(_mediator.Send(new GetCompressedFilesInArchiveImportFolderQuery(folder)));

        await Task.WhenAll(tasks);

        return tasks.SelectMany(_ => _.Result)
            .Distinct()
            .ToList();
    }
}
