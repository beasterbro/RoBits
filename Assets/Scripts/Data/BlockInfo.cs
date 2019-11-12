using System.Collections;
using System.Collections.Generic;

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
}
