using System.Text.RegularExpressions;

using Quickenshtein;

namespace GameManager.Core.MediatR.GameArchiveImporter.Queries;

public record GetMatchingGameForArchiveQuery(string FilePath, List<LocalGame> LocalGames) : IRequest<LocalGame?>;

internal class GetMatchingGameForArchiveQueryHandler : IRequestHandler<GetMatchingGameForArchiveQuery, LocalGame?>
{
    public Task<LocalGame?> Handle(GetMatchingGameForArchiveQuery request, CancellationToken cancellationToken)
    {
        var fileName = Path.GetFileNameWithoutExtension(request.FilePath);
        var sanitizedFileName = SanitizeString(fileName);
        
        if(string.IsNullOrWhiteSpace(sanitizedFileName))
            return Task.FromResult<LocalGame?>(null);

        var likelyMatches = new List<LocalGame>();
        
        foreach ( var game in request.LocalGames )
        {
            // if the sanitized file name is the same as the sanitized archive name, we have a very strong match 
            var sanitizedExistingArchive = SanitizeString(Path.GetFileNameWithoutExtension(game.ArchiveFile));
            if ( sanitizedFileName == sanitizedExistingArchive )
                return Task.FromResult<LocalGame?>(game);
            
            var sanitizedTitle = SanitizeString(game.Title);
            
            if ( string.IsNullOrWhiteSpace(sanitizedTitle) || string.IsNullOrWhiteSpace(sanitizedFileName) )
                continue;

            //if the text is very similar assume that its it probably match
            var titleDistance = Levenshtein.GetDistance(sanitizedTitle, sanitizedFileName);
            var archiveDistance = Levenshtein.GetDistance(sanitizedFileName, sanitizedExistingArchive);
            if ( 
                titleDistance < 2  ||
                archiveDistance < 2  ||
                sanitizedTitle.Contains(sanitizedFileName) || 
                sanitizedFileName.Contains(sanitizedTitle)
                )
            {
                likelyMatches.Add(game);
            }
        }
        
        // from the likely matches, get the one with the highest similarity
        var likelyMatch = likelyMatches
            .OrderBy(game =>
            {
                var sanitizedTitle = SanitizeString(game.Title);
                var sanitizedExistingArchive = SanitizeString(
                    Path.GetFileNameWithoutExtension(game.ArchiveFile)
                );
                return Math.Min(
                    Levenshtein.GetDistance(sanitizedTitle, sanitizedFileName),
                    Levenshtein.GetDistance(sanitizedExistingArchive, sanitizedFileName)
                );
            })
            .FirstOrDefault();

        return Task.FromResult(likelyMatch);
    }
    
    private static string SanitizeString(string title) =>
        Regex.Replace(title, @"[^a-zA-Z]*", "")
            .Trim()
            .ToLower();
}



