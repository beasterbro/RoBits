using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using Newtonsoft.Json;

[JsonConverter(typeof(JsonData.PartConverter))]
public class PartInfo
{

    private readonly int id;
    private readonly string name;
    private readonly string description;
    private readonly PartType type;
    private readonly int price;
    private readonly int levelToUnlock;
    private readonly Dictionary<string, float> attributes;

    public PartInfo(int id, string name, string description, PartType type, int price, int levelToUnlock,
        Dictionary<string, float> attributes)
    {
        this.id = id;
        this.name = name;
        this.description = description;
        this.type = type;
        this.price = price;
        this.levelToUnlock = levelToUnlock;
        this.attributes = attributes;
    }

    public int ID => id;
    public string Name => name;
    public string ResourceName => Regex.Replace(name, "[\\W]", "");
    public string Description => description;
    public PartType PartType => type;
    public int Price => price;
    public int LevelToUnlock => levelToUnlock;
    public Dictionary<string, float> Attributes => attributes;

}