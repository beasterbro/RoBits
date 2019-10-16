using System;
using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class PartConverter : Converter<PartInfo>
{

    public override void SerializeJson(SerializationHelper serializer, PartInfo obj)
    {
        serializer.WriteKeyValue<int>("pid", obj.GetID());
        serializer.WriteKeyValue<string>("name", obj.GetName());
        serializer.WriteKeyValue<string>("desc", obj.GetDescription());
        serializer.WriteKeyValue<string>("type", obj.GetPartType().ToString().ToLower());
        serializer.WriteKeyValue<int>("price", obj.GetPrice());
        serializer.WriteKeyValue<int>("unlockLvl", obj.GetLevelToUnlock());
        serializer.SerializeKeyValue<Dictionary<string, double>>("stats", obj.GetAttributes());
    }

    public override PartInfo DeserializeJson(DeserializationHelper helper)
    {
        int id = helper.GetValue<int>("pid");
        string name = helper.GetValue<string>("name", "");
        string desc = helper.GetValue<string>("desc", "");
        string partString = helper.GetValue<string>("type", "");
        int price = helper.GetValue<int>("price", 100000);
        int unlockLevel = helper.GetValue<int>("unlockLvl", 1000);
        Dictionary<string, double> stats = helper.GetValue<Dictionary<string, double>>("string", new Dictionary<string, double>());

        PartType partType = (PartType)Enum.Parse(typeof(PartType), partString, true);
        return new PartInfo(id, name, desc, partType, price, unlockLevel, false, stats);
    }

}