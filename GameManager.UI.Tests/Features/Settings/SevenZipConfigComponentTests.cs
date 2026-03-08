using GameManager.UI.Features.Settings.Components;
using GameManager.UI.Features.Settings.Actions.Save;

namespace GameManager.UI.Tests.Features.Settings;

/// <summary>
/// Tests for SevenZipConfigComponent.
/// Verifies that the 7-Zip path field is rendered and that SaveSettingsAction is
/// dispatched when the form is submitted.
/// </summary>
public class SevenZipConfigComponentTests : BUnitTestBase
{
    private void SetupSettingsState(string sevenZipPath = @"C:\7-Zip\7z.exe")
    {
        SetupState(new SettingsState
        {
            Settings = new AppSettings { SevenZipPath = sevenZipPath },
            Initialized = true
        });
    }

    [Fact]
    public void RendersSevenZipCard()
    {
        SetupSettingsState();

        var cut = RenderComponent<SevenZipConfigComponent>();

        cut.Markup.Should().Contain("7Zip Install Location");
    }

    [Fact]
    public void RendersPathLabel()
    {
        SetupSettingsState();

        var cut = RenderComponent<SevenZipConfigComponent>();

        cut.Markup.Should().Contain("Path");
    }

    [Fact]
    public void RendersSaveButton()
    {
        SetupSettingsState();

        var cut = RenderComponent<SevenZipConfigComponent>();

        cut.Markup.Should().Contain("Save");
    }

    [Fact]
    public void DispatchesSaveSettingsActionWhenFormSubmitted()
    {
        SetupSettingsState();

        var cut = RenderComponent<SevenZipConfigComponent>();

        cut.Find("form").Submit();

        DispatcherMock.Received().Dispatch(Arg.Any<SaveSettingsAction>());
    }
}
