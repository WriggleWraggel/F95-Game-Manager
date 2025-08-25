using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using GameManager.Core.Data.Settings;
using MediatR;

using Newtonsoft.Json.Linq;

namespace GameManager.Core.Services;

public interface ILocalGameRepo
{
    Task<List<LocalGame>> GameGamesInRootFolders(List<GameLibraryFolder> RootFolders, CancellationToken cancellationToken);

    Task<LocalGame> SaveGame(LocalGame game, CancellationToken cancellationToken);
}

class LocalGameJsonFileRepo : ILocalGameRepo
{
    public readonly NewtonSoftJsonSerializerWrapper _wrapper;

    public LocalGameJsonFileRepo(NewtonSoftJsonSerializerWrapper wrapper) => _wrapper = wrapper;

    public async Task<List<LocalGame>> GameGamesInRootFolders(List<GameLibraryFolder> rootFolders, CancellationToken cancellationToken)
    {
        var gameDataFiles = new List<string>();
        foreach ( var folder in rootFolders.Select(_ => _.Path) )
        {
            gameDataFiles.AddRange(Directory.GetFiles(folder, SettingsConsts.GameDataFileName, SearchOption.AllDirectories));
        }

        var loadTasks = gameDataFiles
            .Where(_ => _ != null)
            .Select(_ => GetGameFromGameJsonFile(_, cancellationToken));

        var res = await Task.WhenAll(loadTasks);

        return res.ToList();
    }

    public async Task<LocalGame> GetGameFromGameJsonFile(string gameJsonFilePath, CancellationToken cancellationToken)
    {
        LocalGame Game = new();
        using ( StreamReader file = File.OpenText(gameJsonFilePath) )
        using ( JsonTextReader reader = new JsonTextReader(file) )
        {
            var jObj = (JObject) await JToken.ReadFromAsync(reader, cancellationToken);
            Game = jObj.ToObject<LocalGame>(_wrapper.Serializer) ??
                throw new Exception($"Unable To Get Game File or deserialize from {gameJsonFilePath}");
        }

        Game.Saved = true;
        return Game;
    }

    public async Task<LocalGame> SaveGame(LocalGame game, CancellationToken cancellationToken)
    {
        var jsonData = JObject.FromObject(game, _wrapper.Serializer);

        game.Saved = true;
        var folderPath = Path.Combine(
            game.RootFolder.Path,
            game.FolderName ?? throw new Exception("Game Folder Name is null")
        );

        if ( !Directory.Exists(folderPath) )
            Directory.CreateDirectory(folderPath);

        var filePath = Path.Combine(folderPath, SettingsConsts.GameDataFileName);
        using ( var file = File.CreateText(filePath) )
        using ( var writer = new JsonTextWriter(file) )
        {
            writer.Formatting = Formatting.Indented;
            await jsonData.WriteToAsync(writer, cancellationToken);
        }

        return game;
    }
}
