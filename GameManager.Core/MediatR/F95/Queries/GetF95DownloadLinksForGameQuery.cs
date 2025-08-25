namespace GameManager.Core.MediatR.F95.Queries;

public class GetF95DownloadLinksForGameQuery : IRequest<List<F95DownloadLink>>
{
    public F95Game F95Game { get; init; } = new F95Game();
}

internal class GetF95DownloadLinksForGameQueryHandler : IRequestHandler<GetF95DownloadLinksForGameQuery, List<F95DownloadLink>>
{
    internal class UnmaskResponse
    {
        public string? Status { get; set; }
        [JsonProperty("msg")]
        public string? Url { get; set; }
    }

    private readonly CookieSession _cookieSession;

    public GetF95DownloadLinksForGameQueryHandler(HttpSessionWrapper wrapper) => _cookieSession = wrapper.Session;

    public async Task<List<F95DownloadLink>> Handle(GetF95DownloadLinksForGameQuery request, CancellationToken cancellationToken)
    {
        if ( request.F95Game == null )
            throw new ArgumentNullException(nameof(request.F95Game));

        var gamePageContent = await _cookieSession
            .Request(HttpConsts.F95Root, "threads", request.F95Game.Id)
            .GetStringAsync(cancellationToken: cancellationToken);

        var gamePageHtml = new HtmlDocument();
        gamePageHtml.LoadHtml(gamePageContent);

        if ( gamePageContent.Contains("You must be registered to see the links") )
            throw new Exception("Not Logged into f95");

        var downloadLinks = new List<F95DownloadLink>();

        //attempt to sanitize the download section to allow parsing
        var downloadsNode = gamePageHtml.DocumentNode
            .SelectSingleNode("//*[text() = 'DOWNLOAD']");

        while ( downloadsNode.Name != "div" )
            downloadsNode = downloadsNode.ParentNode;

        var spans = downloadsNode.ChildNodes.Where(n => n.Name == "span").ToList();
        foreach ( var node in spans )
            downloadsNode.RemoveChild(node, true);

        var iamges = downloadsNode.ChildNodes.Where(n => n.Attributes["class"]?.Value == "js-lbImage").ToList();
        foreach ( var node in iamges )
            downloadsNode.RemoveChild(node);

        var lineBreakNodesInJointSpan =
            downloadsNode
            .ChildNodes
            .Where(_ => _.Name == "br")
            .ToList();

        //for each line in the span
        var currentIndexOfLineBreak = 0;

        //The last known heading text before a download link
        var parentHeading = "";
        for ( var i = 0; i < lineBreakNodesInJointSpan.Count; i++ )
        {
            var nextLineBreakIndex = downloadsNode
                .ChildNodes
                .IndexOf(lineBreakNodesInJointSpan[i]);

            var lineOfDownloadLinks = downloadsNode
                .ChildNodes
                .ToList()
                .GetRange(currentIndexOfLineBreak, nextLineBreakIndex - currentIndexOfLineBreak);

            var downloadLinksHeading = lineOfDownloadLinks
                .FirstOrDefault(_ => _.Name == "b")?
                .InnerText
                .Trim();

            downloadLinksHeading = downloadLinksHeading?.Replace("DOWNLOAD", "");

            var probableDownloadLinks = lineOfDownloadLinks
                .Where(_ => _.Name == "a").ToList();

            //if theres no download links in a row, take the inner text and set it to parent
            if ( !probableDownloadLinks.Any() && !string.IsNullOrWhiteSpace(downloadLinksHeading) )
            {
                parentHeading = downloadLinksHeading;
            }

            downloadLinks.AddRange(probableDownloadLinks
            .Select(_ => new F95DownloadLink
            {
                DownloadDescriptor = string.IsNullOrWhiteSpace(parentHeading) ?
                $"{downloadLinksHeading} - {_.InnerText}" :
                $"{parentHeading} - {downloadLinksHeading} - {_.InnerText}",
                DownloadUrl = new Url(_.Attributes["href"].Value),
                Game = request.F95Game
            }));

            currentIndexOfLineBreak = nextLineBreakIndex;
        }

        //var formContents = new KeyValuePair<string, string>[]
        //{
        //    new ("xhr", "1"),
        //    new ("download", "1"),
        //};


        //foreach ( var links in downloadLinks.Where(_ => _.F95Masked) )
        //{
        //    //TODO Optimize this
        //    //TODO Handle recaptcha somehow
        //    var res = await links.DownloadUrl
        //        .WithCookies(_cookieSession.Cookies)
        //        .PostUrlEncodedAsync(formContents);
        //    var resJson = await res.GetJsonAsync<UnmaskResponse>();
        //    links.DownloadUrl = resJson.Url;
        //}

        return downloadLinks;
    }
}
