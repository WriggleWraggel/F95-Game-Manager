namespace GameManager.UI.Features.Settings.Actions.Set;

public record SetAppSettingsAction(AppSettings Settings);

internal class SetAppSettingsActionReducer : Reducer<SettingsState, SetAppSettingsAction>
{
    public override SettingsState Reduce(SettingsState state, SetAppSettingsAction action) =>
        state with { Settings = action.Settings };
}
