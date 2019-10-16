using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;

[JsonConverter(typeof(BotConverter))]
public class BotInfo
{

    private int id;
    private string name;
    private int tier;
    private Dictionary<string, string> behavior;
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

    public void AddPart(PartInfo part)
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

    public Dictionary<string, string> GetBehaviors()
    {
        return behavior;
    }

    // TODO: Implement
    // GetBotBehavior
    // SetCodeBlocks
    // IsCpuLimitReached
    // IsMaxPartsReached

}
