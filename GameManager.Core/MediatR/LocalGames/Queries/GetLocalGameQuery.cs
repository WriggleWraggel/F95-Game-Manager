using GameManager.Core.Data.Settings;

using Newtonsoft.Json.Linq;

namespace GameManager.Core.MediatR.LocalGames.Queries;

public class GetLocalGameQuery : IRequest<LocalGame>
{
    public string GameFolderPath { get; init; } = string.Empty;
}

internal class GetLocalGameQueryHandler : IRequestHandler<GetLocalGameQuery, LocalGame>
{
    public readonly NewtonSoftJsonSerializerWrapper _wrapper;

    public GetLocalGameQueryHandler(NewtonSoftJsonSerializerWrapper wrapper) => _wrapper = wrapper;

    public async Task<LocalGame> Handle(GetLocalGameQuery request, CancellationToken cancellationToken)
    {
        LocalGame? Game = null;
        var filePath = Path.Combine(request.GameFolderPath, SettingsConsts.GameDataFileName);
        using ( var file = File.OpenText(filePath) )
        using ( var reader = new JsonTextReader(file) )
        {
            var jObj = (JObject) await JToken.ReadFromAsync(reader, cancellationToken);
            Game = jObj.ToObject<LocalGame>(_wrapper.Serializer) ?? throw new Exception($"Unable To Get Game File or deserialize from {filePath}");
        }

        Game.Saved = true;
        return Game;
    }
}
