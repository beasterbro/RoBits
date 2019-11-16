using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//A class to represent items in an inventory
[CreateAssetMenu]
public class Item : ScriptableObject
{//instead of this for each item you put in the string url that the item stats can be found at
    public string ItemName;
    public Sprite Icon;
    public PartInfo part;
    [SerializeField] string id;
    public int partID;
    public string ID => id;
    public String description;
    public PartType type;
    public int price;
    public int levelToUnlock;
    public Dictionary<String, double> attributes;

    public virtual Item GetCopy()
    {
        return this;
    }

    public virtual void Destroy()
    {
        
    }
    
    private void OnValidate()
    {
        part = new PartInfo(partID,ItemName,description,type,price,levelToUnlock,attributes);
        string path = AssetDatabase.GetAssetPath(this);
        id = AssetDatabase.AssetPathToGUID(path);
    }
}
