using GameManager.UI.Features.Settings;

namespace GameManager.UI.Features.Settings.Actions.Save;

public record SaveSettingsSuccessAction();


internal class SaveSettingsSuccessActionReducer : Reducer<SettingsState, SaveSettingsSuccessAction>
{
    public override SettingsState Reduce(SettingsState state, SaveSettingsSuccessAction action) =>
        state with
        {
            Busy = false
        };
}
