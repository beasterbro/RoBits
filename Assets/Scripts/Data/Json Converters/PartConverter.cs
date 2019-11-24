using System;
using System.Collections.Generic;

namespace JsonData
{
    public class PartConverter : Converter<PartInfo>
    {

        protected override void SerializeJson(SerializationHelper serializer, PartInfo obj)
        {
            serializer.WriteKeyValue("pid", obj.ID);
            serializer.WriteKeyValue("name", obj.Name);
            serializer.WriteKeyValue("desc", obj.Description);
            serializer.WriteKeyValue("type", obj.PartType.ToString().ToLower());
            serializer.WriteKeyValue("price", obj.Price);
            serializer.WriteKeyValue("unlockLvl", obj.LevelToUnlock);
            serializer.SerializeKeyValue("stats", obj.Attributes);
        }

        protected override PartInfo DeserializeJson(DeserializationHelper helper)
        {
            var id = helper.GetValue<int>("pid");
            var name = helper.GetValue("name", "");
            var desc = helper.GetValue("desc", "");
            var partString = helper.GetValue("type", "");
            var price = helper.GetValue("price", 100000);
            var unlockLevel = helper.GetValue("unlockLvl", 1000);
            var stats = helper.GetValue("stats", new Dictionary<string, float>());

            var partType = (PartType)Enum.Parse(typeof(PartType), partString, true);
            return new PartInfo(id, name, desc, partType, price, unlockLevel, stats);
        }

    }
}