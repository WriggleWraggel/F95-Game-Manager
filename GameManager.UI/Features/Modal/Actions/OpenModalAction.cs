namespace GameManager.UI.Features.Modal.Actions;

public record OpenModalAction(string Title, Type Type, Dictionary<string, object>? Parameters = null);

internal class OpenModalActionnEffect : Effect<OpenModalAction>
{
    private readonly IState<ModalState> _modalState;

    public OpenModalActionnEffect(IState<ModalState> modalState) => _modalState = modalState;

    public override async Task HandleAsync(OpenModalAction action, IDispatcher dispatcher)
    {
        var modalRef = _modalState.Value.ModalRef ?? throw new Exception("Common Modal has not being congigured");
        await modalRef.Show();
    }
}

internal class OpenModalActionReducer : Reducer<ModalState, OpenModalAction>
{
    public override ModalState Reduce(ModalState state, OpenModalAction action)
        => state with
        {
            ModalTitle = action.Title,
            ModalBody = action.Type,
            Parameters = action.Parameters ?? new(),
        };
}