using GameManager.Core.MediatR.LocalGames.Queries;
using GameManager.UI.Features.GameArchiveImporter.Components;

namespace GameManager.UI.Features.GameArchiveImporter.Actions.PurgeGameFolder;

public record OpenPurgeGameFolderModalAction(LocalGame Game, List<string> GameFolders);

internal class OpenPurgeGameFolderModalActionEffect : Effect<OpenPurgeGameFolderModalAction>
{
    private readonly IMediator _mediator;

    public OpenPurgeGameFolderModalActionEffect(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task HandleAsync(OpenPurgeGameFolderModalAction action, IDispatcher dispatcher)
    {
        dispatcher.Dispatch(new OpenModalAction("Purge Game Folder", typeof(PurgeGameFolderComponent),
            new Dictionary<string, object> {
                { nameof(PurgeGameFolderComponent.Game), action.Game },
                { nameof(PurgeGameFolderComponent.GameFolders), action.GameFolders },
            }));
    }
}