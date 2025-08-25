namespace GameManager.UI.Features.Modal.Actions;

public record CloseModalAction();

internal class CloseModalActionEffect : Effect<CloseModalAction>
{
    private readonly IState<ModalState> _modalState;
    public CloseModalActionEffect(IState<ModalState> modalState) => _modalState = modalState;

    public override async Task HandleAsync(CloseModalAction action, IDispatcher dispatcher)
    {
        var modalRef = _modalState.Value.ModalRef ?? throw new InvalidOperationException("ModalRef is null");
        await modalRef.Hide();
    }
}