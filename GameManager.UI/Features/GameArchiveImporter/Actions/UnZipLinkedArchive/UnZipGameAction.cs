using GameManager.Core.MediatR.LocalGames.Commands;
using GameManager.UI.Features.Settings;

namespace GameManager.UI.Features.GameArchiveImporter.Actions.UnZipLinkedArchive;

public record UnZipGameAction(LocalGame Game);

internal class UnZipGameActionEffect : Effect<UnZipGameAction>
{
    private readonly IMediator _mediator;
    private readonly IState<SettingsState> _settings;

    public UnZipGameActionEffect(IMediator mediator, IState<SettingsState> settings)
    {
        _mediator = mediator;
        _settings = settings;
    }

    public override async Task HandleAsync(UnZipGameAction action, IDispatcher dispatcher)
    {
        try
        {
            var extractedFolder = await _mediator.Send(
                new UnZipLastDownloadFileCommand(action.Game, _settings.Value.Settings.SevenZipPath)
            );
            dispatcher.Dispatch(new UnZipGameSuccessAction(action.Game, extractedFolder));
        }
        catch ( Exception ex )
        {
            dispatcher.Dispatch(new AddErrorNotificationAction(ex.Message, ex, $"Error unziping {action.Game}"));
        }
    }
}

internal class UnZipGameActionReducer : Reducer<GameLibraryState, UnZipGameAction>
{
    public override GameLibraryState Reduce(GameLibraryState state, UnZipGameAction action)
    {
        var currentGameMetaData = state.GameMetaData.FirstOrDefault(_ => _.Id == action.Game.Id);

        if ( currentGameMetaData == null )
        {
            currentGameMetaData = new GameMetaData
            {
                Id = action.Game.Id,
            };
            state.GameMetaData.Add(currentGameMetaData);
        }

        currentGameMetaData.Processing = true;
        return state with
        {
            GameMetaData = state.GameMetaData
        };
    }
}