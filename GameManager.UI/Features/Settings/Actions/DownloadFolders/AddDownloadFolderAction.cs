using GameManager.UI.Features.Settings;

namespace GameManager.UI.Features.Settings.Actions.DownloadFolders;

public record AddNewDownloadFolderAction();

internal class AddNewDownloadFolderActionReducer : Reducer<SettingsState, AddNewDownloadFolderAction>
{
    public override SettingsState Reduce(SettingsState state, AddNewDownloadFolderAction action)
    {
        state.Settings.ImportFolders.Add(new());
        return state with
        {
            Settings = state.Settings
        };
    }
}