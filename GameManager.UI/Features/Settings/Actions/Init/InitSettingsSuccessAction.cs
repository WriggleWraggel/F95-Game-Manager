namespace GameManager.UI.Features.Settings.Actions.Init;

internal class InitSettingsSuccessAction
{
}

internal class InitSettingsSuccessActionReducer : Reducer<SettingsState, InitSettingsSuccessAction>
{
    public override SettingsState Reduce(SettingsState state, InitSettingsSuccessAction action) =>
        state with
        {
            Initialized = true
        };
}
