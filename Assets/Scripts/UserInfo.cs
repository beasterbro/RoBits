using System.Collections.Generic;
using UnityEngine;

public class UserInfo
{

    private class DbUser
    {
        public string uid;
        public string email;
        public string username;
        public int currency;
        public int xp;
        public int level;
        public bool canCompete;
        public Dictionary<string, string> settings;
    }

    private string id;
    private string email;
    private string username;
    private int currency;
    private int xp;
    private int level;
    private bool canCompete;
    private Dictionary<string, string> settings;

    public UserInfo(string id, string email, string username, int currency, int xp, int level, bool canCompete, Dictionary<string, string> settings)
    {
        this.id = id;
        this.email = email;
        this.username = username;
        this.currency = currency;
        this.xp = xp;
        this.level = level;
        this.canCompete = canCompete;
        this.settings = settings;
    }

    public static UserInfo FromJson(string json)
    {
        DbUser db = JsonUtility.FromJson<DbUser>(json);
        return new UserInfo(db.uid, db.email, db.username, db.currency, db.xp, db.level, db.canCompete, db.settings);
    }

    public string GetID()
    {
        return id;
    }

    public string GetEmail()
    {
        return email;
    }

    public string GetUsername()
    {
        return username;
    }

    public int GetCurrency()
    {
        return currency;
    }

    public int GetXP()
    {
        return xp;
    }

    public int GetLevel()
    {
        return level;
    }

    public bool CanCompete()
    {
        return canCompete;
    }

    public Dictionary<string, string> GetSettings()
    {
        return settings;
    }

}
