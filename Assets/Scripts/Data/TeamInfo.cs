using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

[JsonConverter(typeof(JsonData.TeamConverter))]
public class TeamInfo
{

    private readonly int id;
    private string name;
    private DateTime lastMaintained;
    private BotInfo[] bots;
    private double rank;
    private int tier;
    private readonly string userId;
    private UserInfo user;

    public TeamInfo(int id, string name, DateTime lastMaintained, BotInfo[] bots, double rank, int tier, string userId)
    {
        this.id = id;
        this.name = name;
        this.lastMaintained = lastMaintained;
        this.bots = bots;
        this.rank = rank;
        this.tier = tier;
        this.userId = userId;
    }

    public int ID => id;

    public string Name
    {
        get => name;
        set => name = value;
    }

    public DateTime DateLastMaintained => lastMaintained;

    public void SetMaintained()
    {
        lastMaintained = DateTime.Now;
    }

    public BotInfo[] Bots => bots;

    public double Rank
    {
        get => rank;
        set => rank = value;
    }

    public int Tier => tier;
    public string UserID => userId;
    public UserInfo User => user;

    public async Task<bool> FetchUserInfo()
    {
        UserInfo result = await DataManager.Instance.FetchUser(userId);

        if (result != null)
        {
            user = result;
            return true;
        }

        return false;
    }

}