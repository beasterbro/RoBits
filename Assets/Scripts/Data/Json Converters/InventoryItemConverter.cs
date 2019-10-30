namespace JsonData
{
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

            PartInfo part = DataManager.Instance().GetPart(pid);

            return new InventoryItem(part, count);
        }

    }
}
