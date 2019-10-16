using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

[JsonConverter(typeof(PartConverter))]
public class PartInfo
{

    private int id;
    private string name;
    private string description;
    private PartType type;
    private int price;
    private int levelToUnlock;
    private bool isActor;
    private Dictionary<string, double> attributes;
    private Sprite sprite;

    public PartInfo(int id, string name, string description, PartType type, int price, int levelToUnlock, bool isActor, Dictionary<string, double> attributes)
    {
        this.id = id;
        this.name = name;
        this.description = description;
        this.type = type;
        this.price = price;
        this.levelToUnlock = levelToUnlock;
        this.isActor = isActor;
        this.attributes = attributes;
    }

    public int GetID()
    {
        return id;
    }

    public string GetName()
    {
        return name;
    }

    public string GetDescription()
    {
        return description;
    }

    public PartType GetPartType()
    {
        return type;
    }

    public int GetPrice()
    {
        return price;
    }

    public int GetLevelToUnlock()
    {
        return levelToUnlock;
    }

    public bool IsActor()
    {
        return isActor;
    }

    public Dictionary<string, double> GetAttributes()
    {
        return attributes;
    }

    public Sprite GetSprite()
    {
        return sprite;
    }

}
