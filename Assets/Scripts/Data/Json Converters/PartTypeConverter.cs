using System;
using Newtonsoft.Json;

namespace JsonData
{
    public class PartTypeConverter : JsonConverter
    {

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var enumString = (string)reader.Value;
            return Enum.Parse(typeof(PartType), enumString, true);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var t = (PartType)value;
            writer.WriteValue(t.ToString().ToLower());
        }

    }
}