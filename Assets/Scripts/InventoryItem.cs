using Newtonsoft.Json;

[JsonConverter(typeof(InventoryItemConverter))]
public class InventoryItem
{

    private PartInfo part;
    private int count;

    public InventoryItem(PartInfo part, int count)
    {
        this.part = part;
        this.count = count;
    }

    public PartInfo GetPart()
    {
        return part;
    }

    public int GetCount()
    {
        return count;
    }

}
