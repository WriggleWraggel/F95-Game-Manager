namespace GameManager.Core.Http;

public class NewtonSoftJsonSerializerWrapper
{
    public NewtonSoftJsonSerializerWrapper()
    {
        Settings = new JsonSerializerSettings();
        Settings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
        Settings.Converters.Add(new UrlConverter());

        Serializer = JsonSerializer.Create(Settings);
    }
    
    public JsonSerializerSettings Settings { get; }

    public JsonSerializer Serializer { get; }
}

public class UrlConverter : JsonConverter<Url>
{
    public override Url? ReadJson(JsonReader reader, Type objectType, Url? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        string url = (string) (reader.Value ?? "");

        return new Url(url);
    }

    public override void WriteJson(JsonWriter writer, Url? value, JsonSerializer serializer)
    {
        writer.WriteValue(value?.ToString());
    }
}
