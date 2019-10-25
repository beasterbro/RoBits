using System;
using Newtonsoft.Json;

[JsonConverter(typeof(JsonData.TeamConverter))]
public class TeamInfo
{
    private int id;
    private string name;
    private DateTime lastMaintained;
    private BotInfo[] bots;
    private double rank;
    private int tier;

    public TeamInfo(int id, string name, DateTime lastMaintained, BotInfo[] bots, double rank, int tier)
    {
        this.id = id;
        this.name = name;
        this.lastMaintained = lastMaintained;
        this.bots = bots;
        this.rank = rank;
        this.tier = tier;
    }

    public int GetID()
    {
        return id;
    }

    public string GetName()
    {
        return name;
    }

    public void SetName(string name)
    {
        this.name = name;
    }

    public DateTime GetDateLastMaintained()
    {
        return lastMaintained;
    }

    public void SetMaintained()
    {
        lastMaintained = DateTime.Now;
    }

    public BotInfo[] GetBots()
    {
        return bots;
    }

    public double GetRank()
    {
        return rank;
    }

    public void SetRank(double rank)
    {
        this.rank = rank;
    }

    public int GetTier()
    {
        return tier;
    }
}