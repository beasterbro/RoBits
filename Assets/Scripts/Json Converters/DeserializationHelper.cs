using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class DeserializationHelper
{

    private JObject item;

    public DeserializationHelper(JObject item)
    {
        this.item = item;
    }

    public T GetValue<T>(string key)
    {
        if (item[key].GetType() == typeof(JObject) || item[key].GetType() == typeof(JArray))
        {
            return JsonConvert.DeserializeObject<T>(item[key].ToString());
        }
        else
        {
            return item[key].Value<T>();
        }
    }

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