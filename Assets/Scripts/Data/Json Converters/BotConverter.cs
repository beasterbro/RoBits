using System.Linq;
using System.Collections.Generic;

namespace JsonData
{
    public class BotConverter : Converter<BotInfo>
    {

        protected override void SerializeJson(SerializationHelper serializer, BotInfo obj)
        {
            serializer.WriteKeyValue("bid", obj.ID);
            serializer.WriteKeyValue("name", obj.Name);
            serializer.WriteKeyValue("tier", obj.Tier);
            serializer.SerializeKeyValue("parts", obj.Equipment.Select(part => part.ID).ToArray());
            serializer.WriteKeyValue("bodyType", obj.BodyType.ID);
            serializer.SerializeKeyValue("ai", obj.Behaviors);
        }

        protected override BotInfo DeserializeJson(DeserializationHelper helper)
        {
            var id = helper.GetValue<int>("bid");
            var name = helper.GetValue("name", "");
            var partIds = helper.GetValue("parts", new int[0]);
            var bodyTypeId = helper.GetValue("bodyType", 100);
            var tier = helper.GetValue("tier", 0);
            var ai = helper.GetValue("ai", new Dictionary<string, string>());

            var equipment = new List<PartInfo>(partIds.Select(DataManager.Instance.GetPart));
            equipment.RemoveAll(p => p == null);
            var bodyType = DataManager.Instance.GetPart(bodyTypeId);

            return new BotInfo(id, name, tier, equipment, bodyType);
        }

    }
}
