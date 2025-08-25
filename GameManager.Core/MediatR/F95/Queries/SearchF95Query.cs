namespace GameManager.Core.MediatR.F95.Queries;

public class F95SearchResult
{
    public string Status { get; set; } = string.Empty;

    [JsonProperty("msg")]
    public F95SearchResultBody Body { get; set; } = new();

}

public class F95SearchResultBody
{
    [JsonProperty("data")]
    public List<F95Game> Games { get; set; } = new();

    [JsonProperty("pagination")]
    public F95Pagination Pagination { get; set; } = new();

    public int? Count { get; set; }
}

public class F95Pagination
{
    [JsonProperty("page")]
    public int Page { get; set; }

    [JsonProperty("total")]
    public int Total { get; set; }
}

public class SearchF95Query : IRequest<F95SearchResult>
{
    public string Term { get; init; } = string.Empty;
    public List<F95GamePrefix> Prefixes { get; init; } = new();
    public List<F95Tag> Tags { get; init; } = new();
    public int Page { get; init; } = 1;
    public string Sort { get; init; } = "date"; //TODO MAKE ENUM
    public TagType TagType { get; init; } = TagType.Or;
}

public enum TagType
{
    Or,
    And
}

internal class SearchF95QueryHandler : IRequestHandler<SearchF95Query, F95SearchResult>
{
    private readonly CookieSession _cookieSession;
    public SearchF95QueryHandler(HttpSessionWrapper wrapper)
    {
        _cookieSession = wrapper.Session;
    }

    public async Task<F95SearchResult> Handle(SearchF95Query request, CancellationToken cancellationToken)
    {
        //"cmd=list&cat=games&page=1&search=test&sort=date"
        var query = _cookieSession
            .Request(HttpConsts.F95Root, "sam/latest_alpha/latest_data.php?")
            .SetQueryParam("cmd", "list")
            .SetQueryParam("cat", "games")
            .SetQueryParam("prefixes[]", request.Prefixes.Cast<int>())
            .SetQueryParam("tags[]", request.Tags.Cast<int>())
            .SetQueryParam("page", request.Page)
            .SetQueryParam("search", request.Term)
            .SetQueryParam("sort", request.Sort)
            .SetQueryParam("tagtype", request.TagType.ToString().ToLower());

        var res = await query.GetJsonAsync<F95SearchResult>(cancellationToken: cancellationToken);
        res.Body.Games.ForEach(_ => _.Version = _.Version.TrimStart('v', 'V'));
        res.Body.Count ??= 0;

        return res;

    }
}

