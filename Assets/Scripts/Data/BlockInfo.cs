using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

[JsonConverter(typeof(JsonData.BlockConverter))]
public class BlockInfo
{

    private int id;
    private readonly string type;
    private Dictionary<string, string> typeAttrs;
    private int[] inputIDs;
    private int[] chunkSizes;

    public BlockInfo(int id, string type, Dictionary<string, string> typeAttrs, int[] inputIDs, int[] chunkSizes)
    {
        this.id = id;
        this.type = type;
        this.typeAttrs = typeAttrs;
        this.inputIDs = inputIDs;
        this.chunkSizes = chunkSizes;
    }

    public int ID
    {
        get => id;
        set => this.id = value;
    }

    public string Type => type;

    public Dictionary<string, string> TypeAttrs => typeAttrs;

    public int[] InputIDs
    {
        get => inputIDs;
        set => inputIDs = value;
    }

    public int[] ChunkSizes
    {
        get => chunkSizes;
        set => chunkSizes = value;
    }
    
    public override string ToString()
    {
        return string.Format("{1}Info(ID = {0}, Children:[{2}]", ID, Type.ToUpper(), string.Join(" ", InputIDs)) + (ChunkSizes.Length > 0 ? string.Format(", Chunks:[{0}])", string.Join(" ", ChunkSizes)) : ")");
    }
}
