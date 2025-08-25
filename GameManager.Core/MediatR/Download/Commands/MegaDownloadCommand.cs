using CG.Web.MegaApiClient;

namespace GameManager.Core.MediatR.Download.Commands;

public class MegaDownloadCommand : IRequest<string>
{

    public string DestinationFolder { get; init; } = string.Empty;
    public string Url { get; init; } = string.Empty;
    public IProgress<double> ProgressHandler { get; init; } = new Progress<double>();
}

internal class MegaDownloadCommandHandler : IRequestHandler<MegaDownloadCommand, string>
{
    public async Task<string> Handle(MegaDownloadCommand request, CancellationToken cancellationToken)
    {
        if ( !Directory.Exists(request.DestinationFolder) )
            Directory.CreateDirectory(request.DestinationFolder);

        var client = new MegaApiClient();
        await client.LoginAnonymousAsync();

        Uri fileLink = new Uri(request.Url);
        var node = client.GetNodeFromLink(fileLink);

        var destinationPath = Path.Combine(request.DestinationFolder, node.Name);

        using ( var stream = await client.DownloadAsync(node, request.ProgressHandler, cancellationToken) )
        {
            using ( var fileStream = new FileStream(destinationPath, FileMode.Create) )
            {
                await stream.CopyToAsync(fileStream, cancellationToken);
            }
        }

        client.Logout();

        return destinationPath;
    }
}
