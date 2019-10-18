using Newtonsoft.Json;

[JsonConverter(typeof(JsonData.InventoryItemConverter))]
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

    public void IncreaseCount()
    {
        count++;
    }

    public bool DecreaseCount()
    {
        if (count > 0)
        {
            count--;
            return true;
        }

        return false;
    }

}
