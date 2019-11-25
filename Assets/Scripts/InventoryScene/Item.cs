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
    public PartInfo part;
    public int PartID => part.ID;
    public string ID => part.ID.ToString();
    public string ItemName => part.Name;
    public String Description => part.Description;
    public PartType Type => part.PartType;
    public int Price => part.Price;
    public int LevelToUnlock => part.LevelToUnlock;
    public Dictionary<String, float> attributes => part.Attributes;

    public virtual Item GetCopy()
    {
        return this;
    }

    public virtual void Destroy()
    {
        ;
    }
}