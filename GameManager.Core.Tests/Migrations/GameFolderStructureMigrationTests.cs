using System.IO;
using GameManager.Core.Migrations;

namespace GameManager.Core.Tests.Migrations;

public class GameFolderStructureMigrationIntegrationTests : IDisposable
{
    private ILocalGameRepo _localGameRepo;
    private IFileRepo _fileRepo;
    private LocalGame _testGame;
    private readonly string _testRootFolder;

    public GameFolderStructureMigrationIntegrationTests()
    {
        _testRootFolder = Path.Combine(Path.GetTempPath(), "F95GameManagerTests", Guid.NewGuid().ToString());

        _localGameRepo = Substitute.For<ILocalGameRepo>();
        _localGameRepo
            .GameGamesInRootFolders(Arg.Any<List<GameLibraryFolder>>(), Arg.Any<CancellationToken>())
            .Returns(_ => new List<LocalGame> { _testGame });

        _fileRepo = new FileRepo();

        _testGame = new LocalGame()
        {
            FolderName = "Game of Whores",
            RootFolder = new GameLibraryFolder() { Path = _testRootFolder },
        };

        Directory.CreateDirectory(_testGame.FullPath);
    }

    public void Dispose()
    {
        if (Directory.Exists(_testRootFolder))
            Directory.Delete(_testRootFolder, true);
    }

    [Trait("Category", "Integration")]
    [Fact]
    public async Task ApplyMigrationTest()
    {
        var sut = new GameFolderStructureMigration(_localGameRepo, _fileRepo);

        await sut.ApplyMigration(new AppSettings());

        var expectedGameFolder = Path.Combine(_testGame.FullPath, SettingsConsts.ModsFolderName);
        var expectedSavesFolder = Path.Combine(_testGame.FullPath, SettingsConsts.SavesFolderName);
        var expectedArchiveFolder = Path.Combine(_testGame.FullPath, SettingsConsts.ArchivesFolderName);
        var expectedModsFolder = Path.Combine(_testGame.FullPath, SettingsConsts.ModsFolderName);

        Directory.Exists(expectedGameFolder).Should().BeTrue();
        Directory.Exists(expectedSavesFolder).Should().BeTrue();
        Directory.Exists(expectedArchiveFolder).Should().BeTrue();
        Directory.Exists(expectedModsFolder).Should().BeTrue();
    }

    [Trait("Category", "Integration")]
    [Fact]
    public void MoveArchiveFilesToNewArchiveFolderTest()
    {
        // Arrange
        var sut = new GameFolderStructureMigration(_localGameRepo, _fileRepo);

        // Act
        var result = sut.MoveArchiveFilesToNewArchiveFolder(_testGame);

        // Assert
        var expectedArchiveFolderPath = Path.Combine(_testGame.FullPath, SettingsConsts.ArchivesFolderName);
        Directory.Exists(expectedArchiveFolderPath).Should().BeTrue();

        var archiveFiles = _fileRepo.GetArchiveFilePathsInFolder(_testGame.FullPath, false);
        archiveFiles.Should().BeEmpty();
    }

    [Trait("Category", "Integration")]
    [Fact]
    public void MoveExtraFoldersToNewGameFolderTest()
    {
        // Arrange
        var sut = new GameFolderStructureMigration(_localGameRepo, _fileRepo);
        _fileRepo.MakeFolderSafely(Path.Combine(_testGame.FullPath, SettingsConsts.SavesFolderName));
        _fileRepo.MakeFolderSafely(Path.Combine(_testGame.FullPath, SettingsConsts.ArchivesFolderName));
        _fileRepo.MakeFolderSafely(Path.Combine(_testGame.FullPath, SettingsConsts.ModsFolderName));

        var extraFolder = Path.Combine(_testGame.FullPath, "GameFiles");
        Directory.CreateDirectory(extraFolder);

        var expectedGamePath = Path.Combine(_testGame.FullPath, SettingsConsts.GameFolderName);

        Directory.Exists(expectedGamePath).Should().BeFalse();

        // Act
        var res = sut.MoveExtraFoldersToNewGameFolder(_testGame);

        //Assert
        Directory.Exists(expectedGamePath).Should().BeTrue();
        Directory.EnumerateDirectories(expectedGamePath).Should().NotBeEmpty();
    }

    [Trait("Category", "Integration")]
    [Fact]
    public void MakeModsFolderTest()
    {
        // Arrange
        var sut = new GameFolderStructureMigration(_localGameRepo, _fileRepo);
        var expectedPath = Path.Combine(_testGame.FullPath, SettingsConsts.ModsFolderName);
        Directory.Exists(expectedPath).Should().BeFalse();

        // Act
        sut.MakeModsFolderGame(_testGame);

        // Assert
        Directory.Exists(expectedPath).Should().BeTrue();
    }

    [Trait("Category", "Integration")]
    [Fact]
    public void MakeSavesFolderTest()
    {
        // Arrange
        var sut = new GameFolderStructureMigration(_localGameRepo, _fileRepo);
        var expectedPath = Path.Combine(_testGame.FullPath, SettingsConsts.SavesFolderName);
        Directory.Exists(expectedPath).Should().BeFalse();

        // Act
        sut.MakeSavesFolder(_testGame);

        // Assert
        Directory.Exists(expectedPath).Should().BeTrue();
    }
}
