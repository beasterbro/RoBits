using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace JsonData
{

    // Adds convenience methods for deserializing from JSON
    public class DeserializationHelper
    {

        private readonly JObject item;

        public DeserializationHelper(JObject item)
        {
            this.item = item;
        }

        private bool HasValue(string key)
        {
            return item[key] != null;
        }

        // Directly get a value from the JSON blob
        public T GetValue<T>(string key)
        {
            // Confirm the item has the given key
            if (!HasValue(key)) 
                throw new KeyNotFoundException("Attempted to get value for missing key: '" + key + "'");

            // If not a plain value, serialize
            if (item[key].GetType() == typeof(JObject) || item[key].GetType() == typeof(JArray))
            {
                return JsonConvert.DeserializeObject<T>(item[key].ToString());
            }

            // Otherwise, just return the basic value
            return item[key].Value<T>();
        }

        // Wrapper for the above method, providing a default fallback if the property does not exist
        public T GetValue<T>(string key, T defaultValue)
        {
            if (!HasValue(key)) return defaultValue;
            
            try
            {
                return GetValue<T>(key);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return defaultValue;
            }
        }

        public T[] GetArrayValue<T>(string key) => GetArrayValue(key, new T[0]);

        public T[] GetArrayValue<T>(string key, T[] defaultValue)
        {
            if (item[key].GetType() == typeof(JArray))
            {
                return item[key].Children().Select(token => JsonConvert.DeserializeObject<T>(token.ToString())).ToArray();
            }

            return defaultValue;
        }

        public JObject GetItem()
        {
            return item;
        }

    }

}