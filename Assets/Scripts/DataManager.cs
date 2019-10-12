using System;
using System.Collections;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.Networking;

using DB;

// Collects and manages necessary information that needs to be taken from the backend to the frontend and vice versa.
public class DataManager
{

    public static DataManager shared = new DataManager();

    private HttpClient api = new HttpClient();

    private int userCurrency, userXP;
    private string currentUserID;
    private TeamInfo[] userTeams;
    private InventoryItem[] inventory;

    public DataManager() {
        api.BaseAddress = new Uri("http://robits.us-east-2.elasticbeanstalk.com");
        api.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public void EstablishAuth(string token)
    {
        api.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    public int GetUserCurrency()
    {
        return userCurrency;
    }

    public void SetUserCurrency(int amount)
    {
        // TODO: Implement
    }

    public int GetUserLevel()
    {
        // TODO: Implement
        return 0;
    }

    public void AddExperienceToUser(int xp)
    {
        // TODO: Implement
    }

    public InventoryItem[] GetUserInventory()
    {
        // TODO: Implement
        return new InventoryItem[0];
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

    public TeamInfo[] GetUserBotTeams()
    {
        // TODO: Implement
        return new TeamInfo[0];
    }

    public bool UpdateUserBot(BotInfo bot)
    {
        // TODO: Implement
        return true;
    }

    public async Task FetchInitialUserData()
    {
        HttpResponseMessage response = await api.GetAsync("/api/user");

        if (response.IsSuccessStatusCode)
        {
            DBUser userObj = JsonUtility.FromJson<DBUser>(await response.Content.ReadAsStringAsync());
            this.userCurrency = userObj.currency;
            this.userXP = userObj.xp;
            this.currentUserID = userObj.uid;
        }
    }

    public void UpdateUserData()
    {
        // TODO: Implement
    }

    public string GetCurrentUserID()
    {
        return currentUserID;
    }

}
