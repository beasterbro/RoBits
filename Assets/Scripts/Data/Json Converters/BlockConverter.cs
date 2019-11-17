using System.Collections.Generic;

namespace JsonData
{

    public class BlockConverter : Converter<BlockInfo>
    {

        protected override void SerializeJson(SerializationHelper serializer, BlockInfo obj)
        {
            serializer.WriteKeyValue("id", obj.ID);
            serializer.WriteKeyValue("type", obj.Type);
            serializer.WriteKeyValue("typeAttrs", obj.TypeAttrs);
            serializer.WriteKeyValue("inputIds", obj.InputIds);
            serializer.WriteKeyValue("chunkSizes", obj.ChunkSizes);
        }

        protected override BlockInfo DeserializeJson(DeserializationHelper helper)
        {
            var id = helper.GetValue<int>("id");
            var type = helper.GetValue<string>("type");
            var typeAttrs = helper.GetValue("typeAttrs", new Dictionary<string, string>());
            var inputIds = helper.GetValue("inputIds", new int[0]);
            var chunkSizes = helper.GetValue("chunkSizes", new int[0]);
            
            return new BlockInfo(id, type, typeAttrs, inputIds, chunkSizes);
        }

    }

}