using System;
using System.Collections.Generic;
using Newtonsoft.Json;

[JsonConverter(typeof(JsonData.UserConverter))]
public class UserInfo
{

    private readonly string id;
    private readonly string email;
    private string username;
    private int currency;
    private int xp;
    private bool canCompete;
    private readonly Dictionary<string, string> settings;

    public UserInfo(string id, string email, string username, int currency, int xp, bool canCompete,
        Dictionary<string, string> settings)
    {
        this.id = id;
        this.email = email;
        this.username = username;
        this.currency = currency;
        this.xp = xp;
        this.canCompete = canCompete;
        this.settings = settings;
    }

    public string ID => id;
    public string Email => email;
    public string Username => username;

    public int Currency
    {
        get => currency;
        set => currency = value;
    }

    public int XP
    {
        get => xp;
        set => xp = value;
    }

    public int Level => (int) Math.Floor(xp / 1000f);
    public bool CanCompete => canCompete;
    public Dictionary<string, string> Settings => settings;

}