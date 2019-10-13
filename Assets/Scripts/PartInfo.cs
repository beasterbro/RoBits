using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class PartInfo
{

    private class DbPart
    {
        public int pid;
        public string name;
        public PartType type;
        public int price;
        public int unlockLvl;
        public Dictionary<string, double> stats;
    }

    private int id;
    private string name;
    private string description;
    private PartType type;
    private int price;
    private int levelToUnlock;
    private bool isActor;
    private Dictionary<string, double> attributes;
    private Sprite sprite;

    public PartInfo(int id, string name, string description, PartType type, int price, int levelToUnlock, bool isActor, Dictionary<string, double> attributes, Sprite sprite) {
        this.id = id;
        this.name = name;
        this.description = description;
        this.type = type;
        this.price = price;
        this.levelToUnlock = levelToUnlock;
        this.isActor = isActor;
        this.attributes = attributes;
        this.sprite = sprite;
    }

    public static PartInfo FromJson(string json) {
        DbPart db = JsonUtility.FromJson<DbPart>(json);
        return new PartInfo(db.pid, db.name, "", db.type, db.price, db.unlockLvl, false, db.stats, null);
    }

    public static PartInfo[] FromJsonArray(string json) {
        string[] elements = Regex.Split(json.Substring(1, json.Length - 2).Trim(), "(?<=}),(?={\"pid\":)");
        PartInfo[] parts = new PartInfo[elements.Length];

        for (int i = 0;i < elements.Length;i++) {
            parts[i] = PartInfo.FromJson(elements[i].Trim());
        }

        return parts;
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
