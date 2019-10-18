using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using JsonData;

// Collects and manages necessary information that needs to be taken from the backend to the frontend and vice versa.
public class DataManager
{

    private static DataManager shared;

    private HttpClient api = new HttpClient();

    private UserInfo currentUser;
    private PartInfo[] allParts;
    private List<InventoryItem> inventory;

    private BotInfo[] allBots;
    private TeamInfo[] userTeams;


    public DataManager()
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
    public static DataManager GetManager()
    {
        if (shared == null)
        {
            shared = new DataManager();
        }
        return shared;
    }

    // Fetches all necessary initial data
    public async Task FetchInitialData()
    {
        await FetchCurrentUser();
        await FetchAllParts();
        await FetchUserInventory();
        await FetchUserTeams();
    }

    public async Task FetchCurrentUser()
    {
        HttpResponseMessage response = await api.GetAsync("/api/user");

        if (response.IsSuccessStatusCode)
        {
            string json = await response.Content.ReadAsStringAsync();
            currentUser = JsonUtils.DeserializeObject<UserInfo>(json);
        }
    }

    public async Task FetchAllParts()
    {
        HttpResponseMessage response = await api.GetAsync("/api/parts");

        if (response.IsSuccessStatusCode)
        {
            allParts = JsonUtils.DeserializeArray<PartInfo>(await response.Content.ReadAsStringAsync());
        }
    }

    // Must be called after calling FetchAllParts
    public async Task FetchUserInventory()
    {
        HttpResponseMessage response = await api.GetAsync("/api/inventory");

        if (response.IsSuccessStatusCode)
        {
            inventory = new List<InventoryItem>(JsonUtils.DeserializeArray<InventoryItem>(await response.Content.ReadAsStringAsync()));
        }
    }

    // Must be called after calling FetchAllParts
    public async Task FetchUserTeams()
    {
        HttpResponseMessage botsResponse = await api.GetAsync("/api/bots");
        HttpResponseMessage teamsResponse = await api.GetAsync("/api/teams");

        if (botsResponse.IsSuccessStatusCode && teamsResponse.IsSuccessStatusCode)
        {
            allBots = JsonUtils.DeserializeArray<BotInfo>(await botsResponse.Content.ReadAsStringAsync());
            userTeams = JsonUtils.DeserializeArray<TeamInfo>(await teamsResponse.Content.ReadAsStringAsync());
        }
    }

    public async Task UpdateCurrentUser()
    {
        HttpContent updateBody = JsonUtils.SerializeObject(currentUser);
        HttpResponseMessage updateResponse = await api.PostAsync("/api/user", updateBody);
    }

    public UserInfo GetCurrentUser()
    {
        return currentUser;
    }

    public List<InventoryItem> GetUserInventory()
    {
        return inventory;
    }

    public bool RemoveItemFromUserInventory(PartInfo item)
    {
        // TODO: Implement
        return true;
    }

    public bool AddItemToUserInventory(PartInfo item)
    {
        // TODO: Implement
        return true;
    }

    public async Task UpdateUserBot(BotInfo bot)
    {
        HttpContent updateBody = JsonUtils.SerializeObject(bot);
        HttpResponseMessage updateResponse = await api.PostAsync("/api/bots/" + bot.GetID(), updateBody);
    }

    public PartInfo[] GetAllParts()
    {
        return allParts;
    }

    public PartInfo GetPart(int pid)
    {
        foreach (PartInfo part in allParts)
        {
            if (part.GetID() == pid) return part;
        }

        return null;
    }

    public BotInfo[] GetAllBots()
    {
        return allBots;
    }

    public BotInfo GetBot(int bid)
    {
        foreach (BotInfo bot in allBots)
        {
            if (bot.GetID() == bid) return bot;
        }

        return null;
    }

    public TeamInfo[] GetUserTeams()
    {
        return userTeams;
    }

}