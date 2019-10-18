using System;
using System.Collections.Generic;
using Newtonsoft.Json;

[JsonConverter(typeof(JsonData.UserConverter))]
public class UserInfo
{

    private string id;
    private string email;
    private string username;
    private int currency;
    private int xp;
    private bool canCompete;
    private Dictionary<string, string> settings;

    public UserInfo(string id, string email, string username, int currency, int xp, bool canCompete, Dictionary<string, string> settings)
    {
        this.id = id;
        this.email = email;
        this.username = username;
        this.currency = currency;
        this.xp = xp;
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

    public void SetCurrency(int currency)
    {
        this.currency = currency;
    }

    public int GetXP()
    {
        return xp;
    }

    public void SetXP(int xp)
    {
        this.xp = xp;
    }

    public int GetLevel()
    {
        return (int)Math.Floor(((float)xp) / 1000f);
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
