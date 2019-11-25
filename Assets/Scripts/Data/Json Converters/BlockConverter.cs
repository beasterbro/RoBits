using System.Collections.Generic;

namespace JsonData
{

    public class BlockConverter : Converter<BlockInfo>
    {

        protected override void SerializeJson(SerializationHelper serializer, BlockInfo obj)
        {
            serializer.WriteKeyValue("id", obj.ID);
            serializer.WriteKeyValue("type", obj.Type);
            serializer.SerializeKeyValue("typeAttrs", obj.TypeAttrs);
            serializer.SerializeKeyValue("inputIds", obj.InputIds);
            serializer.SerializeKeyValue("chunkSizes", obj.ChunkSizes);
        }

        protected override BlockInfo DeserializeJson(DeserializationHelper helper)
        {
            var id = helper.GetValue<int>("id");
            var type = helper.GetValue<string>("type");
            var typeAttrs = helper.GetValue("typeAttrs", new Dictionary<string, string>());
            var inputIds = helper.GetArrayValue<int>("inputIds");
            var chunkSizes = helper.GetArrayValue<int>("chunkSizes");
            
            return new BlockInfo(id, type, typeAttrs, inputIds, chunkSizes);
        }

    }

}