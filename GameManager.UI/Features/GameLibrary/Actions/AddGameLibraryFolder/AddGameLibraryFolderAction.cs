using GameManager.UI.Features.Settings;

namespace GameManager.UI.Features.GameLibrary.Actions.AddGameLibraryFolder;

public record AddGameLibraryFolderAction { }

internal class AddGameLibraryFolderActionReducer : Reducer<SettingsState, AddGameLibraryFolderAction>
{
    public override SettingsState Reduce(SettingsState state, AddGameLibraryFolderAction action)
    {
        state.Settings.GameLibrarySettings.Folders.Add(new());
        return state with
        {
            Settings = state.Settings,
        };
    }
}