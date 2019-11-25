using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

//A class to visually represent PartInfo to the user
[CreateAssetMenu]
public class Item : ScriptableObject
{
    [Range(1,1000)]
    public int MaximumStacks = 1;
    public Sprite icon;

    private PartInfo _partInfo;
    public PartInfo Part
    {
        get
        {
            if (InventoryItem == null)
            {
                return _partInfo;
            }
            else
            {
                return InventoryItem.Part;
            }
        }
        set => _partInfo = value;
    }

    public int PartID => Part.ID;
    public string ID => Part.ID.ToString();
    public string ItemName => Part.Name;
    public String Description => Part.Description;
    public PartType Type => Part.PartType;
    public int Price => Part.Price;
    public int LevelToUnlock => Part.LevelToUnlock;
    public Dictionary<String, float> Attributes => Part.Attributes;

    private InventoryItem _inventoryItem;
    public InventoryItem InventoryItem
    {
        get
        {
            if (_inventoryItem == null)
            {
                _inventoryItem = new InventoryItem(_partInfo,1);
                return _inventoryItem;
            }
            else
            {
                return _inventoryItem;
            }
        }
        set =>_inventoryItem = value;
    }

    public int Amount => InventoryItem.Count ;

    public virtual Item GetCopy()
    {
        return this;
    }
}