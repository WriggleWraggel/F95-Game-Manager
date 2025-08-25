namespace GameManager.Core.MediatR.F95.Queries;

public record GetF95PageSummaryDataQuery(F95Game Game) : IRequest<F95SummaryData>;

class GetF95PageSummaryDataQueryHandler : IRequestHandler<GetF95PageSummaryDataQuery, F95SummaryData>
{
    private readonly IF95Client _client;

    public GetF95PageSummaryDataQueryHandler(IF95Client client) => _client = client;

    public Task<F95SummaryData> Handle(GetF95PageSummaryDataQuery request, CancellationToken cancellationToken) =>
        _client.GetF95PageSummaryData(request.Game, cancellationToken);
}
