using System.Collections.Generic;

public class UserConverter : Converter<UserInfo>
{

    public override void SerializeJson(SerializationHelper serializer, UserInfo obj)
    {
        serializer.WriteKeyValue<string>("uid", obj.GetID());
        serializer.WriteKeyValue<string>("email", obj.GetEmail());
        serializer.WriteKeyValue<string>("username", obj.GetUsername());
        serializer.WriteKeyValue<int>("currency", obj.GetCurrency());
        serializer.WriteKeyValue<int>("xp", obj.GetXP());
        serializer.WriteKeyValue<int>("level", obj.GetLevel());
        serializer.WriteKeyValue<bool>("canCompete", obj.CanCompete());
        serializer.SerializeKeyValue<Dictionary<string, string>>("settings", obj.GetSettings());
    }

    public override UserInfo DeserializeJson(DeserializationHelper helper)
    {
        string id = helper.GetValue<string>("uid");
        string email = helper.GetValue<string>("email");
        string username = helper.GetValue<string>("username");
        int currency = helper.GetValue<int>("currency", 0);
        int xp = helper.GetValue<int>("xp", 0);
        int level = helper.GetValue<int>("level", 0);
        bool canCompete = helper.GetValue<bool>("canCompete", false);
        Dictionary<string, string> settings = helper.GetValue<Dictionary<string, string>>("settings", new Dictionary<string, string>());

        return new UserInfo(id, email, username, currency, xp, level, canCompete, settings);
    }

}