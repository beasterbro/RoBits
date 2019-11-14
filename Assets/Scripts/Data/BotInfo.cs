﻿using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

[JsonConverter(typeof(JsonData.BotConverter))]
public class BotInfo
{

    private int id;
    private string name;
    private int tier;
    private readonly Dictionary<string, string> behavior;
    private readonly List<PartInfo> equipment;
    private PartInfo bodyType;

    public BotInfo(int id, string name, int tier, IEnumerable<PartInfo> equipment, PartInfo bodyType)
    {
        this.id = id;
        this.name = name;
        this.tier = tier;
        this.equipment = new List<PartInfo>(equipment);
        this.bodyType = bodyType;
    }

    public int ID
    {
        get => id;
        set => id = value;
    }

    public string Name
    {
        get => name;
        set => name = value;
    }

    public int Tier
    {
        get => tier;
        set => tier = value;
    }

    public PartInfo[] Equipment => equipment.ToArray();

    public void AddPart(PartInfo part)
    {
        equipment.Add(part);
    }

    public bool RemovePart(PartInfo part)
    {
        return equipment.Remove(part);
    }

    public PartInfo BodyType
    {
        get => bodyType;
        set => bodyType = value;
    }

    public Dictionary<string, string> Behaviors => behavior;
    
    // TODO: Implement
    // GetBotBehavior
    // SetCodeBlocks
    // IsCpuLimitReached
    // IsMaxPartsReached

}