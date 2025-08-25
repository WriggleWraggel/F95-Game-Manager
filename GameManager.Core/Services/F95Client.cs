using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using GameManager.Core.MediatR.F95.Queries;

using MediatR;

namespace GameManager.Core.Services;

internal interface IF95Client
{
    Task<F95SummaryData> GetF95PageSummaryData(F95Game Game, CancellationToken cancellationToken);
}

internal class F95Client : IF95Client
{
    public async Task<F95SummaryData> GetF95PageSummaryData(F95Game Game, CancellationToken cancellationToken)
    {
        if ( Game == null )
            throw new ArgumentNullException(nameof(Game));

        try
        {
            var gamePageContent = await Game.GameUrl
                 .GetStringAsync(cancellationToken: cancellationToken);

            var gamePageHtml = new HtmlDocument();
            gamePageHtml.LoadHtml(gamePageContent);

            //Thread Updated date seems to correspond to the date the version was released
            var updatedNode = gamePageHtml.DocumentNode
                .SelectSingleNode("//*[contains(text(), 'Thread Updated')]");

            //if we cant get the thread updated try getting the release date
            updatedNode ??= gamePageHtml.DocumentNode
                .SelectSingleNode("//*[contains(text(), 'Release')]");

            if ( updatedNode == null )
                throw new Exception($"Couldn't find Updated date in {Game.GameUrl}");

            //first look for the text Thread Updated and get the assoicated date, else get the first time node
            DateTime? updatedDate =
                DateTime.TryParse(GetF95HeadingText(updatedNode), out var parsedDateTime) ?
                parsedDateTime :
                DateTime.TryParse(gamePageHtml.DocumentNode.SelectSingleNode("//time")?.GetAttributeValue("datetime", null), out parsedDateTime) ?
                    parsedDateTime : null;

            var versionNode = gamePageHtml.DocumentNode
                .SelectSingleNode("//*[contains(text(), 'Version')]");

            if ( versionNode == null )
                throw new Exception($"Couldn't find Thread Version node in {Game.GameUrl}");

            var version = GetF95HeadingText(versionNode);

            return new F95SummaryData
            {
                Id = Game.Id,
                UpdateDate = updatedDate,
                Version = version,
            };
        }
        catch ( Exception ex )
        {
            throw new F95GamePageException(Game, ex.Message, ex);
        }
    }

    private string? GetF95HeadingText(HtmlNode node) => node.NextSibling?.InnerText?.Replace(":", "")?.Trim();
}

public class F95GamePageException : Exception
{
    public F95Game ErroredGame { get; init; }

    public F95GamePageException(F95Game erroredGame, string message, Exception innerException)
        : base(message, innerException)
    {
        ErroredGame = erroredGame;
    }
}