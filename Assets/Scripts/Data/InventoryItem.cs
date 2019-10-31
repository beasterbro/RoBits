using Newtonsoft.Json;

[JsonConverter(typeof(JsonData.InventoryItemConverter))]
public class InventoryItem
{

    private readonly PartInfo part;
    private int count;

    public InventoryItem(PartInfo part, int count)
    {
        this.part = part;
        this.count = count;
    }

    public PartInfo Part => part;

    public int Count => count;

    /*
     *  increases the count of this specific inventory item
     */
    public void IncreaseCount()
    {
        count++;
    }

    /*
     * Attempts to decrease the count of this specific inventory item that this user has
     * returns false if the user has no more of this specific inventory item
     */
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