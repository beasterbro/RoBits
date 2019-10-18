using System;
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
            else
            {
                // Otherwise, just return the basic value
                return item[key].Value<T>();
            }
        }

        // Wrapper for the above method, providing a default fallback if the property
        // does not exxist
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

        public JObject GetItem()
        {
            return item;
        }

    }
}