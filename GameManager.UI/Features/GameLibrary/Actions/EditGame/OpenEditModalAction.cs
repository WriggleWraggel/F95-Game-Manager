using System.Text.RegularExpressions;

using GameManager.Core.MediatR.LocalGames.Queries;
using GameManager.UI.Features.GameLibrary.Controls;

namespace GameManager.UI.Features.GameLibrary.Actions.EditGame;

public record OpenEditModalAction(LocalGame Game);

internal class OpenEditModalActionEffect : Effect<OpenEditModalAction>
{
    private readonly IMediator _mediator;

    public OpenEditModalActionEffect(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task HandleAsync(OpenEditModalAction action, IDispatcher dispatcher)
    {
        try
        {
            var folderData = action.Game.Saved ?
                await _mediator.Send(new GetPossibleGameFilesFromDirectoryQuery(action.Game))
                    ?? new FolderData { PossibleGameFiles = new(), PossibleLastDownloadFiles = new() } :
                new FolderData { PossibleGameFiles = new(), PossibleLastDownloadFiles = new() };

            action.Game.ArchiveFile = string.IsNullOrWhiteSpace(action.Game.ArchiveFile) ?
                folderData.PossibleLastDownloadFiles.FirstOrDefault() ?? "" :
                action.Game.ArchiveFile;

            action.Game.LaunchExePath = string.IsNullOrWhiteSpace(action.Game.LaunchExePath) ?
                folderData.PossibleGameFiles.FirstOrDefault() ?? "" :
                action.Game.LaunchExePath;

            if ( !string.IsNullOrWhiteSpace(action.Game.ArchiveFile) && string.IsNullOrWhiteSpace(action.Game.Version) )
            {
                var match = Regex.Match(Path.GetFileNameWithoutExtension(action.Game.ArchiveFile), Consts.VersionPattern1);
                if ( match.Success )
                    action.Game.Version = match.Groups[0].Value;

                var match2 = Regex.Match(action.Game.Version, Consts.VersionPattern2);
                if ( match2.Success )
                    action.Game.Version = match2.Groups[0].Value;

                action.Game.Version = action.Game.Version.TrimStart('v');
            }

            dispatcher.Dispatch(new OpenModalAction("Edit Game", typeof(ManagedGameEditControl),
                new Dictionary<string, object> {
                    { nameof(ManagedGameEditControl.Game), action.Game },
                    { nameof(ManagedGameEditControl.LastDownloadFileSelection), folderData.PossibleLastDownloadFiles },
                    { nameof(ManagedGameEditControl.PossibleGameFileSelection), folderData.PossibleGameFiles },
                    {
                        nameof(ManagedGameEditControl.OnSubmit),
                        new EventCallback<LocalGame>(null,  (LocalGame _) => {
                            dispatcher.Dispatch(new SaveLocalGameAction(_));
                            dispatcher.Dispatch(new CloseModalAction());
                        })
                    },
                    {
                        nameof(ManagedGameEditControl.AutoSaveEvent),
                        new EventCallback<LocalGame>(null,  (LocalGame _) => {
                            dispatcher.Dispatch(new SaveLocalGameAction(_));
                        })
                    },
                }));
        }
        catch ( Exception ex )
        {
            dispatcher.Dispatch(new AddErrorNotificationAction(ex.Message, ex, 
                $"Error opening edit modal for {action.Game.Title}")
            );
        }
    }
}

