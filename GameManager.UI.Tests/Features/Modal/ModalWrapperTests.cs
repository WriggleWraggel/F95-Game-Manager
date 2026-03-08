using GameManager.UI.Features.Modal.Components;

namespace GameManager.UI.Tests.Features.Modal;

/// <summary>
/// Tests for ModalWrapper.
/// Verifies that the modal title from ModalState is rendered and that the dynamic
/// body component is rendered using the type stored in ModalState.
/// </summary>
public class ModalWrapperTests : BUnitTestBase
{
    [Fact]
    public void RendersModalTitle()
    {
        SetupState(new ModalState
        {
            ModalTitle = "Test Modal Title",
            ModalBody = typeof(ModalBodyPlaceHolder),
            Parameters = new Dictionary<string, object>()
        });

        var cut = RenderComponent<ModalWrapper>();

        cut.Markup.Should().Contain("Test Modal Title");
    }

    [Fact]
    public void RendersDynamicBodyComponent()
    {
        SetupState(new ModalState
        {
            ModalTitle = "Dynamic Modal",
            ModalBody = typeof(ModalBodyPlaceHolder),
            Parameters = new Dictionary<string, object>()
        });

        var cut = RenderComponent<ModalWrapper>();

        // ModalBodyPlaceHolder renders its h3
        cut.Markup.Should().Contain("ModalBodyPlaceHolder");
    }
}
