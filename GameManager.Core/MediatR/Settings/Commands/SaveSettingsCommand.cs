
using System.Reflection;

using GameManager.Core.Data.Settings;

using Newtonsoft.Json.Linq;

namespace GameManager.Core.MediatR.Settings.Commands;

public class SaveSettingsCommand : IRequest
{
    public AppSettings? AppSettings { get; init; }
}

internal class SaveSettingsCommandHandler : IRequestHandler<SaveSettingsCommand>
{
    public readonly NewtonSoftJsonSerializerWrapper _wrapper;
    private readonly ISettingsPathProvider _settingsPathProvider;

    public SaveSettingsCommandHandler(NewtonSoftJsonSerializerWrapper wrapper, ISettingsPathProvider settingsPathProvider)
    {
        _wrapper = wrapper;
        _settingsPathProvider = settingsPathProvider;
    }

    public async Task Handle(SaveSettingsCommand request, CancellationToken cancellationToken)
    {
        if ( request.AppSettings == null )
            throw new ArgumentNullException(nameof(request.AppSettings));

        var settings = request.AppSettings;

        settings.ImportFolders = settings.ImportFolders
            .Where(_ => !string.IsNullOrWhiteSpace(_.Path))
            .ToList();

        settings.GameLibrarySettings.Folders = settings.GameLibrarySettings.Folders
            .Where(_ => !string.IsNullOrWhiteSpace(_.Path))
            .ToList();

        var jsonData = JObject.FromObject(settings, _wrapper.Serializer);

        var filePath = Path.Combine(_settingsPathProvider.Path, SettingsConsts.SettingsFileName);
        using ( var file = File.CreateText(filePath) )
        using ( var writer = new JsonTextWriter(file) )
        {
            writer.Formatting = Formatting.Indented;
            await jsonData.WriteToAsync(writer, cancellationToken);
        }
    }
}
