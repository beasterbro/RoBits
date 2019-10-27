using System.Text;
using System.Net.Http;
using Newtonsoft.Json;

namespace JsonData
{
    public class JsonUtils
    {

        // A wrapper class for serializing/deserializing top-level arrays
        [System.Serializable]
        private class Wrapper<T>
        {
            public T[] Items = new T[0];
        }

        public static T DeserializeObject<T>(string json, params JsonConverter[] converters)
        {
            return JsonConvert.DeserializeObject<T>(json, converters);
        }

        public static T[] DeserializeArray<T>(string json, params JsonConverter[] converters)
        {
            Wrapper<T> wrapped = JsonConvert.DeserializeObject<Wrapper<T>>(json, converters);
            return wrapped.Items;
        }

        public static HttpContent SerializeObject(object obj, params JsonConverter[] converters)
        {
            string json = JsonConvert.SerializeObject(obj, converters);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

    }
}