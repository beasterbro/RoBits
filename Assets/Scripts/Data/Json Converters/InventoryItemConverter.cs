namespace JsonData
{

    public class InventoryItemConverter : Converter<InventoryItem>
    {

        protected override void SerializeJson(SerializationHelper serializer, InventoryItem obj)
        {
            serializer.WriteKeyValue("pid", obj.Part.ID);
            serializer.WriteKeyValue("count", obj.Count);
        }

        protected override InventoryItem DeserializeJson(DeserializationHelper helper)
        {
            int pid = helper.GetValue<int>("pid");
            int count = helper.GetValue("count", 0);

            PartInfo part = DataManager.Instance.GetPart(pid);

            return new InventoryItem(part, count);
        }

    }

}