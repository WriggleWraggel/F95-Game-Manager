namespace GameManager.UI.Features.GameUpdater.Actions.UpdateProgress;

public record IncreaseUpdateProgresAction(int Increase);

internal class IncreaseUpdateProgresActionReducer : Reducer<GameUpdaterState, IncreaseUpdateProgresAction>
{
    public override GameUpdaterState Reduce(GameUpdaterState state, IncreaseUpdateProgresAction action) =>
        state with
        {
            GamesCheckedProgress = state.GamesCheckedProgress + action.Increase
        };
}
