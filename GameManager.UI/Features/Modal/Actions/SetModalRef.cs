namespace GameManager.UI.Features.Modal.Actions;

public record SetModalRef(Blazorise.Modal ModalRef);


internal class SetModalRefReducer : Reducer<ModalState, SetModalRef>
{
    public override ModalState Reduce(ModalState state, SetModalRef action)
        => state with
        {
            ModalRef = action.ModalRef
        };
}