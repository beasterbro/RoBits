using System;
using UnityEngine;

public class TeamInfo
{

    private class DbTeam
    {
        public int tid;
        public string name;
        public string last_maintenance;
        public double rank;
        public int tier;
    }

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

    public static TeamInfo FromJson(string json, BotInfo[] bots)
    {
        DbTeam db = JsonUtility.FromJson<DbTeam>(json);
        return new TeamInfo(db.tid, db.name, DateTime.Parse(db.last_maintenance), new BotInfo[3], db.rank, db.tier);
    }

    public static TeamInfo FromJson(string json)
    {
        return TeamInfo.FromJson(json, new BotInfo[3]);
    }

    public int GetID()
    {
        return id;
    }

    public string GetName()
    {
        return name;
    }

    public DateTime GetDateLastMaintained()
    {
        return lastMaintained;
    }

    public BotInfo[] GetBots()
    {
        return bots;
    }

    public double GetRank()
    {
        return rank;
    }

    public int GetTier()
    {
        return tier;
    }

}