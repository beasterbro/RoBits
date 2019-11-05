using System.Collections.Generic;

namespace JsonData
{
    public class UserConverter : Converter<UserInfo>
    {

        protected override void SerializeJson(SerializationHelper serializer, UserInfo obj)
        {
            serializer.WriteKeyValue("uid", obj.ID);
            serializer.WriteKeyValue("email", obj.Email);
            serializer.WriteKeyValue("username", obj.Username);
            serializer.WriteKeyValue("currency", obj.Currency);
            serializer.WriteKeyValue("xp", obj.XP);
            serializer.WriteKeyValue("canCompete", obj.CanCompete);
            serializer.SerializeKeyValue("settings", obj.Settings);
        }

        protected override UserInfo DeserializeJson(DeserializationHelper helper)
        {
            var id = helper.GetValue<string>("uid");
            var email = helper.GetValue<string>("email");
            var username = helper.GetValue<string>("username");
            var currency = helper.GetValue("currency", 0);
            var xp = helper.GetValue("xp", 0);
            var canCompete = helper.GetValue("canCompete", false);
            var settings = helper.GetValue("settings", new Dictionary<string, string>());

            return new UserInfo(id, email, username, currency, xp, canCompete, settings);
        }

    }
}