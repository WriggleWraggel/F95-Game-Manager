using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;

using Bunit.TestDoubles;

using Microsoft.Extensions.DependencyInjection;

namespace GameManager.UI.Tests;

/// <summary>
/// Base class for all BUnit component tests. Registers Blazorise, Fluxor state mocks,
/// and a dispatcher mock so that derived test classes can focus on component-specific
/// setup and assertions.
/// </summary>
public abstract class BUnitTestBase : TestContext
{
    protected IDispatcher DispatcherMock { get; }
    protected IStore StoreMock { get; }

    protected BUnitTestBase()
    {
        // Allow Blazorise JS interop calls to succeed silently in tests
        JSInterop.Mode = JSRuntimeMode.Loose;

        // Register Blazorise with Bootstrap 5 and Font Awesome
        Services
            .AddBlazorise(options => { options.Immediate = true; })
            .AddBootstrap5Providers()
            .AddFontAwesomeIcons();

        // IDispatcher mock – individual tests can verify dispatched actions
        DispatcherMock = Substitute.For<IDispatcher>();
        Services.AddSingleton(DispatcherMock);

        // IStore mock – required by FluxorComponent lifecycle
        StoreMock = Substitute.For<IStore>();
        Services.AddSingleton(StoreMock);

        // IActionSubscriber mock – required by FluxorComponent
        var actionSubscriberMock = Substitute.For<IActionSubscriber>();
        Services.AddSingleton(actionSubscriberMock);
    }

    /// <summary>
    /// Registers a mocked <see cref="IState{TState}"/> that returns the given value.
    /// </summary>
    protected IState<TState> SetupState<TState>(TState value) where TState : class
    {
        var stateMock = Substitute.For<IState<TState>>();
        stateMock.Value.Returns(value);
        Services.AddSingleton(stateMock);
        return stateMock;
    }

    /// <summary>
    /// Creates a default <see cref="AppSettings"/> with at least one library folder and
    /// one import folder, suitable for most component tests.
    /// </summary>
    protected static AppSettings DefaultSettings(
        string libraryFolderPath = @"C:\Games",
        string importFolderPath = @"C:\Downloads") => new()
    {
        GameLibrarySettings = new GameLibrarySettings
        {
            Folders = new List<GameLibraryFolder> { new() { Path = libraryFolderPath } }
        },
        ImportFolders = new List<ArchiveImportFolder> { new() { Path = importFolderPath } },
        AuthSettings = new F95AuthSettings { Username = "testuser", Password = "testpass" },
        SevenZipPath = @"C:\7-Zip\7z.exe"
    };

    /// <summary>
    /// Creates a minimal <see cref="LocalGame"/> suitable for tests.
    /// </summary>
    protected static LocalGame CreateGame(
        string title = "Test Game",
        string version = "1.0",
        bool saved = true,
        bool updateAvailable = false,
        string launchPath = "",
        string archiveFile = "",
        GameLibraryFolder? rootFolder = null) => new()
    {
        Title = title,
        Version = version,
        Saved = saved,
        UpdateAvailable = updateAvailable,
        LaunchExePath = launchPath,
        ArchiveFile = archiveFile,
        RootFolder = rootFolder ?? new GameLibraryFolder { Path = @"C:\Games" },
        FolderName = title
    };

    /// <summary>
    /// Creates a minimal <see cref="F95Game"/> suitable for tests.
    /// </summary>
    protected static F95Game CreateF95Game(
        int id = 1,
        string title = "F95 Game",
        string version = "2.0") => new()
    {
        Id = id,
        Title = title,
        Version = version,
        CoverUrl = new Flurl.Url("https://example.com/cover.jpg"),
        ThreadLastUpdatedDate = new DateTime(2024, 1, 1)
    };
}
