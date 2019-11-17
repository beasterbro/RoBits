namespace JsonData
{

    public class BehaviorConverter: Converter<BehaviorInfo>
    {

        protected override void SerializeJson(SerializationHelper serializer, BehaviorInfo obj)
        {
            serializer.WriteKeyValue("triggerId", obj.TriggerId);
            serializer.WriteKeyValue("entryBlockId", obj.EntryBlockId);
            serializer.SerializeKeyValue("blocks", obj.Blocks);
        }

        protected override BehaviorInfo DeserializeJson(DeserializationHelper helper)
        {
            var triggerId = helper.GetValue<int>("triggerId");
            var entryBlockId = helper.GetValue<int>("entryBlockId");
            var blocks = helper.GetArrayValue<BlockInfo>("blocks");

            return new BehaviorInfo(triggerId, entryBlockId, blocks);
        }

    }

}