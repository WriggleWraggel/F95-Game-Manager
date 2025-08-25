using GameManager.Core.Data.Settings;

using Newtonsoft.Json.Linq;

namespace GameManager.Core.MediatR.Settings.Queries;

public record GetSettingsQuery : IRequest<AppSettings>;

internal class GetSettingsQueryHandler : IRequestHandler<GetSettingsQuery, AppSettings>
{
    private readonly NewtonSoftJsonSerializerWrapper _wrapper;
    private readonly ISettingsPathProvider _settingsPathProvider;

    public GetSettingsQueryHandler(NewtonSoftJsonSerializerWrapper wrapper, ISettingsPathProvider settingsPathProvider)
    {
        _wrapper = wrapper;
        _settingsPathProvider = settingsPathProvider;
    }

    public async Task<AppSettings> Handle(GetSettingsQuery request, CancellationToken cancellationToken)
    {
        var settings = new AppSettings();

        var filePath = Path.Combine(_settingsPathProvider.Path, SettingsConsts.SettingsFileName);
        using ( var file = File.OpenText(filePath) )
        using ( var reader = new JsonTextReader(file) )
        {
            var jObj = (JObject) await JToken.ReadFromAsync(reader, cancellationToken);

            if ( jObj == null )
                throw new Exception("unable to get JObject");

            settings = jObj.ToObject<AppSettings>(_wrapper.Serializer) ?? throw new Exception($"Unable to get settings file or de-serialize from {filePath}");
        }

        return settings;
    }
}
