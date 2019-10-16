using Newtonsoft.Json;

public class JsonUtils
{

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] Items = new T[0];
    }

    public static T DeserializeObject<T>(string json)
    {
        return JsonConvert.DeserializeObject<T>(json);
    }

    public static T DeserializeObject<T>(string json, params JsonConverter[] converters)
    {
        return JsonConvert.DeserializeObject<T>(json, converters);
    }

    public static T[] DeserializeArray<T>(string json)
    {
        Wrapper<T> wrapped = JsonConvert.DeserializeObject<Wrapper<T>>(json);
        return wrapped.Items;
    }

    public static T[] DeserializeArray<T>(string json, params JsonConverter[] converters)
    {
        Wrapper<T> wrapped = JsonConvert.DeserializeObject<Wrapper<T>>(json, converters);
        return wrapped.Items;
    }

}