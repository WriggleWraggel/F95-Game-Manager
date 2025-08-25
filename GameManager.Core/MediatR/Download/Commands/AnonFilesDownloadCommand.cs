namespace GameManager.Core.MediatR.Download.Commands;

public class AnonFilesDownloadCommand : IRequest<string>
{
    public string DestinationFolder { get; init; } = string.Empty;
    public string Url { get; init; } = string.Empty;
    public IProgress<double> ProgressHandler { get; init; } = new Progress<double>();
}

internal class AnonFilesDownloadCommandHandler : IRequestHandler<AnonFilesDownloadCommand, string>
{
    public async Task<string> Handle(AnonFilesDownloadCommand request, CancellationToken cancellationToken)
    {

        if ( !Directory.Exists(request.DestinationFolder) )
            Directory.CreateDirectory(request.DestinationFolder);

        var url = new Url(request.Url);
        var html = await url.GetStringAsync(cancellationToken: cancellationToken);

        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);

        var destinationUrl = new Url(htmlDoc
            .DocumentNode
            .SelectSingleNode("//*[@id='download-url']")
            .Attributes["href"].Value);

        var fileName = new Url(htmlDoc
            .DocumentNode
            .SelectSingleNode("//h1")
            .InnerText);

        var destinationPath = Path.Combine(request.DestinationFolder, fileName);

        var downloadclient = new HttpClient();

        using ( var fileStream = new FileStream(destinationPath, FileMode.Create) )
        {
            await downloadclient.DownloadDataAsync(destinationUrl, fileStream, request.ProgressHandler, cancellationToken: cancellationToken);
        }

        return destinationPath;
    }
}
