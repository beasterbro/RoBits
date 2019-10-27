using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu]
public class Item : ScriptableObject
{//instead of this for each item you put in the string url that the item stats can be found at
    public string ItemName;
    public Sprite Icon;
    public PartInfo part;
    [SerializeField] string id;
    public int partID;
    public string ID
    {
        get { return id; }
    }
    public String description;
    public PartType type;
    public int price;
    public int levelToUnlock;
    public bool isActor;
    public Dictionary<String, double> attributes;

    private void OnValidate()
    {
        part = new PartInfo(partID,ItemName,description,type,price,levelToUnlock,isActor,attributes);
        string path = AssetDatabase.GetAssetPath(this);
        id = AssetDatabase.AssetPathToGUID(path);
    }
}
