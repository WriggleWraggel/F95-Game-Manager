using GameManager.Core.MediatR.LocalGames.Commands;
using GameManager.Core.MediatR.LocalGames.Queries;
using GameManager.UI.Features.GameArchiveImporter.Actions.PurgeGameFolder;
using GameManager.UI.Features.Settings;

namespace GameManager.UI.Features.GameArchiveImporter.Actions.UnZipLinkedArchive;

public record PreProcessUnZipGameAction(LocalGame Game);

internal class PreProcessUnZipGameActionEffect : Effect<PreProcessUnZipGameAction>
{
    private readonly IMediator _mediator;
    private readonly IState<SettingsState> _settings;

    public PreProcessUnZipGameActionEffect(IMediator mediator, IState<SettingsState> settings)
    {
        _mediator = mediator;
        _settings = settings;
    }

    public override async Task HandleAsync(PreProcessUnZipGameAction action, IDispatcher dispatcher)
    {
        try
        {
            switch (action.Game.GameEngine )
            {
                // when its a renpy game, prompt the user to purge the game folder
                case GameEngine.Renpy:
                    var gameFolders = await _mediator.Send(new GetGameDirectoryFoldersQuery(action.Game));
                    if ( gameFolders.Count != 0 )
                    {
                        
                        dispatcher.Dispatch(new OpenPurgeGameFolderModalAction(action.Game, gameFolders));
                    }
                    else
                    {
                        dispatcher.Dispatch(new UnZipGameAction(action.Game));
                    }
                    break;
                case GameEngine.Unity:
                case GameEngine.Html:
                case GameEngine.Rpgm:
                case GameEngine.Flash:
                case GameEngine.Java:
                case GameEngine.Qsp:
                case GameEngine.Rags:
                case GameEngine.Unreal:
                case GameEngine.WolfRpg:
                case GameEngine.Unknown:
                default:
                    dispatcher.Dispatch(new UnZipGameAction(action.Game));
                    break;
            }
        }
        catch ( Exception ex )
        {
            dispatcher.Dispatch(new AddErrorNotificationAction(ex.Message, ex, $"Error unziping {action.Game}"));
        }
    }
}