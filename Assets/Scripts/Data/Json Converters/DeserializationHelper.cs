using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JsonData
{

    // Adds convenience methods for deserializing from JSON
    public class DeserializationHelper
    {

        private JObject item;

        public DeserializationHelper(JObject item)
        {
            this.item = item;
        }

        // Directly get a value from the JSON blob
        public T GetValue<T>(string key)
        {
            // If not a plain value, serialize
            if (item[key].GetType() == typeof(JObject) || item[key].GetType() == typeof(JArray))
            {
                return JsonConvert.DeserializeObject<T>(item[key].ToString());
            }

            // Otherwise, just return the basic value
            return item[key].Value<T>();
        }

        // Wrapper for the above method, providing a default fallback if the property
        // does not exist
        public T GetValue<T>(string key, T defaultValue)
        {
            try
            {
                return GetValue<T>(key);
            }
            catch (NullReferenceException e)
            {
                return defaultValue;
            }
        }

        public T[] GetArrayValue<T>(string key, T[] defaultValue)
        {
            if (item[key].GetType() == typeof(JArray))
            {
                return item[key].Children()
                    .Select(token => JsonConvert.DeserializeObject<T>(token.ToString())).ToArray();
            }

            return defaultValue;
        }

        public JObject GetItem()
        {
            return item;
        }

    }

}