namespace GameManager.UI.Features.GameArchiveImporter.Actions.Map;

public record MapLocalGamesToDetectedFilesSuccessAction(List<FileMap> MappedGames);

internal class MapLocalGamesToDetectedFilesSuccessActionReducer : Reducer<GameArchiveImporterState, MapLocalGamesToDetectedFilesSuccessAction>
{
    public override GameArchiveImporterState Reduce(GameArchiveImporterState state, MapLocalGamesToDetectedFilesSuccessAction action)
    {

        var newColletion = new List<FileMap>();
        newColletion.AddRange(action.MappedGames);

        var unmapped = state.FileMaps.Where(old => newColletion.All(_ => _.FilePath != old.FilePath));

        newColletion.AddRange(unmapped);

        return state with
        {
            FileMaps = newColletion,
        };
    }
}
