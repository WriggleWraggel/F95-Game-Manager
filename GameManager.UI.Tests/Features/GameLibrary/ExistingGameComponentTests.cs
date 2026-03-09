using GameManager.UI.Features.GameLibrary.Components.ExistingGames;
using GameManager.UI.Features.GameLibrary.Actions.EditGame;
using GameManager.UI.Features.GameLibrary.Actions.DeleteGame;
using GameManager.UI.Features.GameLibrary.Actions.RunGame;
using GameManager.UI.Features.GameLibrary.Actions.OpenGameFolder;
using GameManager.UI.Features.GameArchiveImporter.Actions.UnZipLinkedArchive;

namespace GameManager.UI.Tests.Features.GameLibrary;

/// <summary>
/// Tests for ExistingGameComponent.
/// Verifies play button state based on launch path, that action buttons dispatch
/// the correct Fluxor actions, unzip button behaviour based on archive file and
/// 7-Zip configuration, and that the correct F95 link vs search button is shown.
/// </summary>
public class ExistingGameComponentTests : BUnitTestBase
{
    private void SetupDefaultStates(
        AppSettings? settings = null,
        LocalGame? game = null)
    {
        SetupState(new SettingsState { Settings = settings ?? DefaultSettings(), Initialized = true });
        SetupState(new GameLibraryState
        {
            Games = game != null ? new List<LocalGame> { game } : new List<LocalGame>()
        });
    }

    [Fact]
    public void ShowsPlayButtonWhenLaunchPathIsSet()
    {
        var game = CreateGame(launchPath: @"C:\Games\game.exe");
        SetupDefaultStates(game: game);

        var cut = RenderComponent<ExistingGameComponent>(parameters => parameters
            .Add(p => p.Game, game)
            .Add(p => p.Processing, false));

        // Play button should not be disabled
        var playButtons = cut.FindAll("button").Where(b => b.InnerHtml.Contains("play") || b.OuterHtml.Contains("play")).ToList();
        playButtons.Should().NotBeEmpty();
    }

    [Fact]
    public void ShowsDisabledPlayButtonWhenLaunchPathIsEmpty()
    {
        var game = CreateGame(launchPath: "");
        SetupDefaultStates(game: game);

        var cut = RenderComponent<ExistingGameComponent>(parameters => parameters
            .Add(p => p.Game, game)
            .Add(p => p.Processing, false));

        // The play button rendered when path is empty is disabled
        var disabledButtons = cut.FindAll("button[disabled]");
        disabledButtons.Should().NotBeEmpty();
    }

    [Fact]
    public void DispatchesOpenEditModalActionWhenEditClicked()
    {
        var game = CreateGame(title: "Edit Target");
        SetupDefaultStates(game: game);

        var cut = RenderComponent<ExistingGameComponent>(parameters => parameters
            .Add(p => p.Game, game)
            .Add(p => p.Processing, false));

        // Edit button uses fa-pen-to-square icon in FontAwesome 6
        var editButtons = cut.FindAll("button").Where(b => b.OuterHtml.Contains("fa-pen-to-square")).ToList();
        editButtons.Should().NotBeEmpty();
        editButtons.Last().Click();

        DispatcherMock.Received().Dispatch(Arg.Is<OpenEditModalAction>(a => a.Game == game));
    }

    [Fact]
    public void DispatchesOpenDeleteGameModalActionWhenDeleteClicked()
    {
        var game = CreateGame(title: "Delete Target");
        SetupDefaultStates(game: game);

        var cut = RenderComponent<ExistingGameComponent>(parameters => parameters
            .Add(p => p.Game, game)
            .Add(p => p.Processing, false));

        // Delete button uses fa-trash icon
        var deleteButtons = cut.FindAll("button").Where(b => b.OuterHtml.Contains("fa-trash")).ToList();
        deleteButtons.Should().NotBeEmpty();
        deleteButtons.First().Click();

        DispatcherMock.Received().Dispatch(Arg.Is<OpenDeleteGameModalAction>(a => a.Game == game));
    }

    [Fact]
    public void DispatchesOpenGameFolderActionWhenFolderClicked()
    {
        var game = CreateGame(title: "Folder Target");
        SetupDefaultStates(game: game);

        var cut = RenderComponent<ExistingGameComponent>(parameters => parameters
            .Add(p => p.Game, game)
            .Add(p => p.Processing, false));

        var folderButtons = cut.FindAll("button").Where(b => b.OuterHtml.Contains("fa-folder-open")).ToList();
        folderButtons.Should().NotBeEmpty();
        folderButtons.First().Click();

        DispatcherMock.Received().Dispatch(Arg.Is<OpenGameFolderAction>(a => a.Game == game));
    }

    [Fact]
    public void ShowsUnzipButtonWhenArchiveFileIsSet()
    {
        var game = CreateGame(archiveFile: @"C:\Downloads\game_v1.zip");
        SetupDefaultStates(game: game);

        var cut = RenderComponent<ExistingGameComponent>(parameters => parameters
            .Add(p => p.Game, game)
            .Add(p => p.Processing, false));

        cut.Markup.Should().Contain("fa-file-archive");
    }

    [Fact]
    public void UnzipButtonIsDisabledWhenSevenZipPathIsEmpty()
    {
        var settings = DefaultSettings();
        settings.SevenZipPath = "";
        var game = CreateGame(archiveFile: @"C:\Downloads\game_v1.zip");
        SetupDefaultStates(settings: settings, game: game);

        var cut = RenderComponent<ExistingGameComponent>(parameters => parameters
            .Add(p => p.Game, game)
            .Add(p => p.Processing, false));

        // Unzip button should be rendered but disabled
        var disabledButtons = cut.FindAll("button[disabled]");
        disabledButtons.Should().NotBeEmpty();
    }

    [Fact]
    public void ShowsSearchButtonWhenF95GameIsNull()
    {
        var game = CreateGame();
        game.F95Game = null;
        SetupDefaultStates(game: game);

        var cut = RenderComponent<ExistingGameComponent>(parameters => parameters
            .Add(p => p.Game, game)
            .Add(p => p.Processing, false));

        // Search button uses fa-magnifying-glass icon in FontAwesome 6
        cut.Markup.Should().Contain("fa-magnifying-glass");
    }

    [Fact]
    public void ShowsLinkButtonWhenF95GameIsLinked()
    {
        var game = CreateGame();
        game.F95Game = CreateF95Game();
        SetupDefaultStates(game: game);

        var cut = RenderComponent<ExistingGameComponent>(parameters => parameters
            .Add(p => p.Game, game)
            .Add(p => p.Processing, false));

        // External link button uses fa-square-up-right icon in FontAwesome 6
        cut.Markup.Should().Contain("fa-square-up-right");
    }
}
