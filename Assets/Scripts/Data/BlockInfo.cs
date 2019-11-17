using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

[JsonConverter(typeof(JsonData.BlockConverter))]
public class BlockInfo
{

    private readonly int id;
    private readonly string type;
    private readonly Dictionary<string, string> typeAttrs;
    private readonly int[] inputIds;
    private readonly int[] chunkSizes;

    public BlockInfo(int id, string type, Dictionary<string, string> typeAttrs, int[] inputIds, int[] chunkSizes)
    {
        this.id = id;
        this.type = type;
        this.typeAttrs = typeAttrs;
        this.inputIds = inputIds;
        this.chunkSizes = chunkSizes;
    }

    public int ID => id;
    public string Type => type;

    public Dictionary<string, string> TypeAttrs => typeAttrs;

    public int[] InputIds => inputIds;
    public int[] ChunkSizes => chunkSizes;

    public override string ToString()
    {
        return string.Format("{1}Info(ID = {0}, Children:[{2}]", ID, Type.ToUpper(), string.Join(" ", InputIds)) + (ChunkSizes.Length > 0 ? string.Format(", Chunks:[{0}])", string.Join(" ", ChunkSizes)) : ")");
    }
}
