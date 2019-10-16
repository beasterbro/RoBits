using System.Collections.Generic;
using Newtonsoft.Json;

[JsonConverter(typeof(UserConverter))]
public class UserInfo
{

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
