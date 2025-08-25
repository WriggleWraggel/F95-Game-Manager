
using GameManager.UI.Features.Modal.Components;

namespace GameManager.UI.Features.Modal;

[FeatureState]
public record ModalState
{
    public Blazorise.Modal? ModalRef { get; init; }
    public string ModalTitle { get; init; } = "init";
    public Type ModalBody { get; init; } = typeof(ModalBodyPlaceHolder);
    public Dictionary<string, object> Parameters { get; init; } = new();
}