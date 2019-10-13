using System.Collections.Generic;
using UnityEngine;

// TODO: Add custom serializer
public class BotInfo
{

    private class DbBot
    {
        public int bid;
        public string name;
        public int[] parts;
        public int bodyType;
        public int tier;
        public Dictionary<string, string> ai; // TODO: Change to a better data type later
    }

    private int id;
    private string name;
    private int tier;
    // botBehavior
    private List<PartInfo> equipment;
    private PartInfo bodyType;

    public BotInfo(int id, string name, int tier, ICollection<PartInfo> equipment, PartInfo bodyType)
    {
        this.id = id;
        this.name = name;
        this.tier = tier;
        this.equipment = new List<PartInfo>(equipment);
        this.bodyType = bodyType;
    }

    public static BotInfo FromJson(string json)
    {
        DbBot db = JsonUtility.FromJson<DbBot>(json);
        List<PartInfo> parts = new List<PartInfo>();
        PartInfo bodyType = null;

        foreach (int id in db.parts)
        {
            PartInfo part = DataManager.shared.GetPartById(id);
            if (part != null) parts.Add(part);
        }

        return new BotInfo(db.bid, db.name, db.tier, parts, bodyType);
    }

    public int GetID()
    {
        return id;
    }

    public string GetName()
    {
        return name;
    }

    public int GetTier()
    {
        return tier;
    }

    public PartInfo[] GetEquippedParts()
    {
        return equipment.ToArray();
    }

    public bool AddPart(PartInfo part)
    {
        equipment.Add(part);
    }

    public bool RemovePart(PartInfo part)
    {
        return equipment.Remove(part);
    }

    public PartInfo GetBodyType()
    {
        return bodyType;
    }

    // TODO: Implement
    // GetBotBehavior
    // SetCodeBlocks
    // IsCpuLimitReached
    // IsMaxPartsReached

}
