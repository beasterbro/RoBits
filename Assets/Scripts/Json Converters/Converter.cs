using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// A generic class, which provides simpler methods of serializing/deserializing JSON
// objects than are provided by the JsonConverter class
public abstract class Converter<T> : JsonConverter where T : class
{

    // Determines whether the json object can be converted to type T
    public override bool CanConvert(Type objectType)
    {
        return typeof(T).IsAssignableFrom(objectType);
    }

    // Serializes the object to JSON (C# -> JSON)
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        T obj = value as T;
        SerializationHelper helper = new SerializationHelper(writer, serializer);
        helper.Start();
        SerializeJson(helper, obj);
        helper.End();
    }

    // Abstract convenience method for serialization, to be implemented by subclasses
    public abstract void SerializeJson(SerializationHelper serializer, T obj);

    // Deserializes the JSON object (JSON -> C#)
    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.StartObject)
        {
            return DeserializeJson(new DeserializationHelper(JObject.Load(reader)));
        }

        return null;
    }

    // Abstract convenience method for deserialization, to be implemented by subclasses
    public abstract T DeserializeJson(DeserializationHelper helper);

}