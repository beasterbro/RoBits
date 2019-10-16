using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public abstract class Converter<T> : JsonConverter where T : class
{

    public override bool CanConvert(Type objectType)
    {
        return typeof(T).IsAssignableFrom(objectType);
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        T obj = value as T;
        SerializationHelper helper = new SerializationHelper(writer, serializer);
        helper.Start();
        SerializeJson(helper, obj);
        helper.End();
    }

    public abstract void SerializeJson(SerializationHelper serializer, T obj);

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.StartObject)
        {
            return DeserializeJson(new DeserializationHelper(JObject.Load(reader)));
        }

        return null;
    }

    public abstract T DeserializeJson(DeserializationHelper helper);

}