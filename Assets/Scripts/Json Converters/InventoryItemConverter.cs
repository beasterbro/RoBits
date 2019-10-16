using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class InventoryItemConverter : Converter<InventoryItem>
{

    public override void SerializeJson(SerializationHelper serializer, InventoryItem obj)
    {
        serializer.WriteKeyValue<int>("pid", obj.GetPart().GetID());
        serializer.WriteKeyValue<int>("count", obj.GetCount());
    }

    public override InventoryItem DeserializeJson(DeserializationHelper helper)
    {
        int pid = helper.GetValue<int>("pid");
        int count = helper.GetValue<int>("count", 0);

        PartInfo part = null;

        foreach (PartInfo p in DataManager.GetManager().GetAllParts())
        {
            if (p.GetID() == pid)
            {
                part = p;
                break;
            }
        }

        return new InventoryItem(part, count);
    }

}