using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using JsonData;

// Collects and manages necessary information that needs to be taken from the backend to the frontend and vice versa.
public class DataManager
{

    private static DataManager shared;

    private HttpClient api = new HttpClient();
    private bool initialFetchPerformed;

    private UserInfo currentUser;
    private PartInfo[] allParts;
    private List<InventoryItem> inventory;

    private BotInfo[] allBots;
    private TeamInfo[] userTeams;

    private DataManager()
    {
        api.BaseAddress = new Uri("http://robits.us-east-2.elasticbeanstalk.com");
        api.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    // Adds the auth header to the HTTP client
    public void EstablishAuth(string token)
    {
        api.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    // Returns a reference to the shared instance
    public static DataManager Instance => shared ?? (shared = new DataManager());

    public bool InitialFetchPerformed => initialFetchPerformed;

    // Fetches all necessary initial data
    public async Task FetchInitialData()
    {
        await FetchCurrentUser();
        await FetchAllParts();
        await FetchUserInventory();
        await FetchUserTeams();
        initialFetchPerformed = true;
    }

    public async Task FetchCurrentUser()
    {
        var response = await api.GetAsync("/api/user");

        if (response.IsSuccessStatusCode)
        {
            currentUser = JsonUtils.DeserializeObject<UserInfo>(await response.Content.ReadAsStringAsync());
        }
    }

    public async Task FetchAllParts()
    {
        var response = await api.GetAsync("/api/parts");

        if (response.IsSuccessStatusCode)
        {
            allParts = JsonUtils.DeserializeArray<PartInfo>(await response.Content.ReadAsStringAsync());
        }
    }

    // Must be called after calling FetchAllParts
    public async Task FetchUserInventory()
    {
        var response = await api.GetAsync("/api/inventory");

        if (response.IsSuccessStatusCode)
        {
            inventory = new List<InventoryItem>(
                JsonUtils.DeserializeArray<InventoryItem>(await response.Content.ReadAsStringAsync()));
        }
    }

    // Must be called after calling FetchAllParts
    public async Task FetchUserTeams()
    {
        var botsResponse = await api.GetAsync("/api/bots");
        var teamsResponse = await api.GetAsync("/api/teams");

        if (botsResponse.IsSuccessStatusCode && teamsResponse.IsSuccessStatusCode)
        {
            allBots = JsonUtils.DeserializeArray<BotInfo>(await botsResponse.Content.ReadAsStringAsync());
            userTeams = JsonUtils.DeserializeArray<TeamInfo>(await teamsResponse.Content.ReadAsStringAsync());

            foreach (TeamInfo team in userTeams)
            {
                await team.FetchUserInfo();
            }
        }
    }

    public async Task<UserInfo> FetchUser(string uid)
    {
        if (uid == currentUser.ID) return currentUser;

        var response = await api.GetAsync("/api/user/" + uid);

        if (response.IsSuccessStatusCode)
        {
            return JsonUtils.DeserializeObject<UserInfo>(await response.Content.ReadAsStringAsync());
        }

        return null;
    }

    public UserInfo CurrentUser => currentUser;

    public async Task UpdateCurrentUser()
    {
        var updateBody = JsonUtils.SerializeObject(currentUser);
        var updateResponse = await api.PutAsync("/api/user", updateBody);
    }

    public List<InventoryItem> UserInventory => inventory;

    public bool SellPart(PartInfo item)
    {
        // TODO: Implement
        return true;
    }

    public bool PurchasePart(PartInfo item)
    {
        // TODO: Implement
        return true;
    }

    public PartInfo[] AllParts => allParts;

    public PartInfo GetPart(int pid)
    {
        return allParts.First(part => part.ID == pid);
    }

    public BotInfo[] AllBots => allBots;

    public BotInfo GetBot(int bid)
    {
        return allBots.First(bot => bot.ID == bid);
    }

    public async Task UpdateBot(BotInfo bot)
    {
        var updateBody = JsonUtils.SerializeObject(bot);
        var updateResponse = await api.PutAsync("/api/bots/" + bot.ID, updateBody);
    }

    public TeamInfo[] UserTeams => userTeams;

    public TeamInfo GetTeam(int tid)
    {
        return userTeams.First(team => team.ID == tid);
    }

    public async Task UpdateTeam(TeamInfo team)
    {
        var updateBody = JsonUtils.SerializeObject(team);
        var updateResponse = await api.PutAsync("/api/teams/" + team.ID, updateBody);
    }

    public async Task<TeamInfo[]> GetOtherUserTeams(string uid)
    {
        var response = await api.GetAsync("/api/users/" + uid + "/teams?expandBots=true");

        if (response.IsSuccessStatusCode)
        {
            var teams = JsonUtils.DeserializeArray<TeamInfo>(await response.Content.ReadAsStringAsync());

            foreach (var team in teams)
            {
                await team.FetchUserInfo();
            }

            return teams;
        }

        return null;
    }

    public async Task<TeamInfo> GetOtherUserTeam(string uid, int tid)
    {
        var teams = await GetOtherUserTeams(uid);
        return teams.FirstOrDefault(team => team.ID == tid);
    }

}