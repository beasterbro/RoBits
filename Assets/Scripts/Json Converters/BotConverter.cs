using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

        List<PartInfo> equipment = new List<PartInfo>();
        PartInfo bodyType = null;

        foreach (int pid in partIds)
        {
            foreach (PartInfo part in DataManager.GetManager().GetAllParts())
            {
                if (part.GetID() == pid)
                {
                    equipment.Add(part);
                }

                if (part.GetID() == bodyTypeId)
                {
                    bodyType = part;
                }
            }
        }

        return new BotInfo(id, name, tier, equipment, bodyType);
    }

}