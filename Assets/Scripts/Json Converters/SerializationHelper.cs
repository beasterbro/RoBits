using Newtonsoft.Json;

// Adds convenience methods for converting to JSON
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

    // Writes a key/value pair
    public void WriteKeyValue<T>(string key, T value)
    {
        writer.WritePropertyName(key);
        writer.WriteValue(value);
    }

    // Writes a key/value pair for a non-primitive value, which must be serialized
    public void SerializeKeyValue<T>(string key, T value)
    {
        writer.WritePropertyName(key);
        serializer.Serialize(writer, value);
    }

}