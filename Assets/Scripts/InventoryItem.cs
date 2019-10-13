using System;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class InventoryItem
{

    private class DbInventoryItem
    {
        public int count;
    }

    private PartInfo part;
    private int count;

    public InventoryItem(PartInfo part, int count)
    {
        this.part = part;
        this.count = count;
    }

    public static InventoryItem FromJson(string json)
    {
        PartInfo part = PartInfo.FromJson(json);
        DbInventoryItem item = JsonUtility.FromJson<DbInventoryItem>(json);

        return new InventoryItem(part, item.count);
    }

    public static InventoryItem[] FromJsonArray(string json)
    {
        string[] items = Regex.Split(json.Substring(1, json.Length - 2).Trim(), "(?<=}),(?={\"pid\":)");
        InventoryItem[] inventoryItems = new InventoryItem[items.Length];

        for (int i = 0;i < items.Length;i++) {
            inventoryItems[i] = InventoryItem.FromJson(items[i].Trim());
        }

        return inventoryItems;
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
