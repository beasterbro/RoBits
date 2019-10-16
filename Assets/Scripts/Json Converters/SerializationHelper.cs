using Newtonsoft.Json;

public class SerializationHelper
{

    private JsonWriter writer;
    private JsonSerializer serializer;

    public SerializationHelper(JsonWriter writer)
    {
        this.writer = writer;
    }

    public SerializationHelper(JsonWriter writer, JsonSerializer serializer)
    {
        this.writer = writer;
        this.serializer = serializer;
    }

    public void Start()
    {
        writer.WriteStartObject();
    }

    public void End()
    {
        writer.WriteEndObject();
    }

    public void WriteKeyValue<T>(string key, T value)
    {
        writer.WritePropertyName(key);
        writer.WriteValue(value);
    }

    public void SerializeKeyValue<T>(string key, T value)
    {
        writer.WritePropertyName(key);
        serializer.Serialize(writer, value);
    }

}