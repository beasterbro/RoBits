using System.Linq;
using System.Collections.Generic;

namespace JsonData
{
    public class BotConverter : Converter<BotInfo>
    {

        public override void SerializeJson(SerializationHelper serializer, BotInfo obj)
        {
            serializer.WriteKeyValue<int>("bid", obj.GetID());
            serializer.WriteKeyValue<string>("name", obj.GetName());
            serializer.WriteKeyValue<int>("tier", obj.GetTier());
            serializer.SerializeKeyValue<int[]>("parts", obj.GetEquippedParts().Select<PartInfo, int>(part => part.GetID()).ToArray());
            serializer.WriteKeyValue<int>("bodyType", obj.GetBodyType().GetID());
            serializer.SerializeKeyValue<Dictionary<string, string>>("ai", obj.GetBehaviors());
        }

        public override BotInfo DeserializeJson(DeserializationHelper helper)
        {
            int id = helper.GetValue<int>("bid");
            string name = helper.GetValue<string>("name", "");
            int[] partIds = helper.GetValue<int[]>("parts", new int[0]);
            int bodyTypeId = helper.GetValue<int>("bodyType", 0);
            int tier = helper.GetValue<int>("tier", 0);
            Dictionary<string, string> ai = helper.GetValue<Dictionary<string, string>>("ai", new Dictionary<string, string>());

            List<PartInfo> equipment = new List<PartInfo>(partIds.Select<int, PartInfo>(DataManager.instance().GetPart));
            equipment.RemoveAll(p => p == null);
            PartInfo bodyType = DataManager.instance().GetPart(bodyTypeId);

            return new BotInfo(id, name, tier, equipment, bodyType);
        }

    }
}
