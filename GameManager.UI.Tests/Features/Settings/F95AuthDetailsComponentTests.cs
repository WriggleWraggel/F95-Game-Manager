using GameManager.UI.Features.Settings.Components;
using GameManager.UI.Features.Settings.Actions.Save;
using GameManager.UI.Features.F95Login.Actions;

namespace GameManager.UI.Tests.Features.Settings;

/// <summary>
/// Tests for F95AuthDetailsComponent.
/// Verifies that the username/password fields are rendered with correct labels,
/// and that the appropriate Fluxor actions are dispatched for Authenticate and Save.
/// </summary>
public class F95AuthDetailsComponentTests : BUnitTestBase
{
    private void SetupState(AppSettings? settings = null)
    {
        base.SetupState(new SettingsState
        {
            Settings = settings ?? DefaultSettings(),
            Initialized = true
        });
    }

    [Fact]
    public void RendersAuthDetailsCard()
    {
        SetupState();

        var cut = RenderComponent<F95AuthDetailsComponent>();

        cut.Markup.Should().Contain("F95 Auth Details");
    }

    [Fact]
    public void RendersUsernameField()
    {
        SetupState();

        var cut = RenderComponent<F95AuthDetailsComponent>();

        cut.Markup.Should().Contain("Username");
    }

    [Fact]
    public void RendersPasswordField()
    {
        SetupState();

        var cut = RenderComponent<F95AuthDetailsComponent>();

        cut.Markup.Should().Contain("Password");
    }

    [Fact]
    public void RendersAuthenticateButton()
    {
        SetupState();

        var cut = RenderComponent<F95AuthDetailsComponent>();

        cut.Markup.Should().Contain("Authenticate");
    }

    [Fact]
    public void DispatchesSaveSettingsActionWhenFormSubmitted()
    {
        SetupState();

        var cut = RenderComponent<F95AuthDetailsComponent>();

        cut.Find("form").Submit();

        DispatcherMock.Received().Dispatch(Arg.Any<SaveSettingsAction>());
    }

    [Fact]
    public void DispatchesF95AuthActionWhenAuthenticateClicked()
    {
        SetupState();

        var cut = RenderComponent<F95AuthDetailsComponent>();

        var authenticateButton = cut.FindAll("button")
            .FirstOrDefault(b => b.TextContent.Contains("Authenticate"));
        authenticateButton.Should().NotBeNull();
        authenticateButton!.Click();

        DispatcherMock.Received().Dispatch(Arg.Any<F95AuthAction>());
    }
}
