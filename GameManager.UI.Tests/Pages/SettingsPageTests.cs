using SettingsPage = GameManager.UI.Pages.SettingsPage;

namespace GameManager.UI.Tests.Pages;

/// <summary>
/// Tests for the Settings page (/settings).
/// The Settings page renders F95AuthDetailsComponent, LibraryFoldersComponent,
/// DownloadFoldersComponent, and SevenZipConfigComponent for full application
/// configuration.
/// </summary>
public class SettingsPageTests : BUnitTestBase
{
    public SettingsPageTests()
    {
        SetupState(new GameLibraryState());
        SetupState(new SettingsState { Settings = DefaultSettings(), Initialized = true });
        SetupState(new OnlineSearchState
        {
            Search = new F95SearchProperties(),
            Result = new F95SearchResult()
        });
        SetupState(new F95LoginState { IsLoggedIn = true });
        SetupState(new GameUpdaterState());
        SetupState(new GameArchiveImporterState());
        SetupState(new ModalState
        {
            ModalBody = typeof(GameManager.UI.Features.Modal.Components.ModalBodyPlaceHolder),
            Parameters = new Dictionary<string, object>()
        });
    }

    [Fact]
    public void RendersWithoutCrashing()
    {
        var cut = RenderComponent<SettingsPage>();

        cut.Should().NotBeNull();
    }

    [Fact]
    public void RendersSettingsHeading()
    {
        var cut = RenderComponent<SettingsPage>();

        cut.Markup.Should().Contain("Settings");
    }

    [Fact]
    public void RendersF95AuthCard()
    {
        var cut = RenderComponent<SettingsPage>();

        cut.Markup.Should().Contain("F95 Auth Details");
    }

    [Fact]
    public void RendersSevenZipCard()
    {
        var cut = RenderComponent<SettingsPage>();

        cut.Markup.Should().Contain("7Zip Install Location");
    }
}
