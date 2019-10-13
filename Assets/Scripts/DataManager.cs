using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.Networking;

// Collects and manages necessary information that needs to be taken from the backend to the frontend and vice versa.
public class DataManager
{

    private static DataManager shared ;

    private HttpClient api = new HttpClient();

    private UserInfo currentUser;

    private TeamInfo[] userTeams;
    private List<InventoryItem> inventory;
    private PartInfo[] allParts;

    public DataManager()
    {
        api.BaseAddress = new Uri("http://robits.us-east-2.elasticbeanstalk.com");
        api.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public void EstablishAuth(string token)
    {
        api.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    public static DataManager GetManager()
    {
        if (shared == null)
        {
            shared = new DataManager();
        }
        return shared;
    }
    

    public async Task FetchInitialData()
    {
        await FetchCurrentUser();
        await FetchAllParts();
        await FetchUserInventory();
    }

    public async Task FetchCurrentUser()
    {
        HttpResponseMessage response = await api.GetAsync("/api/user");

        if (response.IsSuccessStatusCode)
        {
            string json = await response.Content.ReadAsStringAsync();
            currentUser = UserInfo.FromJson(json);
        }
    }

    public async Task FetchUserInventory()
    {
        HttpResponseMessage response = await api.GetAsync("/api/inventory?expandParts=true");

        if (response.IsSuccessStatusCode)
        {
            inventory = new List<InventoryItem>(InventoryItem.FromJsonArray(await response.Content.ReadAsStringAsync()));
        }
    }

    public async Task FetchAllParts()
    {
        HttpResponseMessage response = await api.GetAsync("/api/parts");

        if (response.IsSuccessStatusCode)
        {
            allParts = PartInfo.FromJsonArray(await response.Content.ReadAsStringAsync());
        }
    }

    public void UpdateUserData()
    {
        // TODO: Implement
    }

    // TODO: Replace with GetUser().GetCurrency()?
    public int GetUserCurrency()
    {
        return currentUser.GetCurrency();
    }

    public void SetUserCurrency(int amount)
    {
        // TODO: Implement
    }

    public int GetUserLevel()
    {
        return currentUser.GetLevel();
    }

    public string GetCurrentUserID()
    {
        return currentUser.GetID();
    }

    public void AddExperienceToUser(int xp)
    {
        // TODO: Implement
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

    public PartInfo GetPartById(int id)
    {
        foreach (PartInfo part in allParts)
        {
            if (part.GetID() == id) return part;
        }

        return null;
    }

    public PartInfo[] GetAllParts()
    {
        return allParts;
    }

}