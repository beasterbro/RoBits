using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JsonData;
using UnityEngine;
using UnityEngine.Networking;

// Collects and manages necessary information that needs to be taken from the backend to the frontend and vice versa.
public class DataManager
{

    private static string baseUrl = "http://robits.us-east-2.elasticbeanstalk.com/api";
    private static DataManager shared;

    private string bearerToken;
    private MonoBehaviour runner;

    private UserInfo currentUser;
    private PartInfo[] allParts;
    private List<InventoryItem> inventory;

    private BotInfo[] allBots;
    private TeamInfo[] userTeams;

    private DataManager() { }

    public void Latch(MonoBehaviour coroutineRunner)
    {
        runner = coroutineRunner;
    }

    private UnityWebRequest WrapRequest(UnityWebRequest request)
    {
        request.SetRequestHeader("Authorization", "Bearer " + bearerToken);
        request.SetRequestHeader("Accept", "application/json");
        return request;
    }

    private UnityWebRequest BasicGet(string endpt)
    {
        return WrapRequest(UnityWebRequest.Get(baseUrl + endpt));
    }

    private UnityWebRequest BasicPost(string endpt, string content)
    {
        return WrapRequest(UnityWebRequest.Post(baseUrl + endpt, content));
    }

    private UnityWebRequest BasicPut(string endpt, string content)
    {
        return WrapRequest(UnityWebRequest.Put(baseUrl + endpt, content));
    }

    private UnityWebRequest BasicDelete(string endpt)
    {
        return WrapRequest(UnityWebRequest.Delete(baseUrl + endpt));
    }

    // Adds the auth header to the HTTP client
    public void EstablishAuth(string token)
    {
        bearerToken = token;
    }

    // Returns a reference to the shared instance
    public static DataManager Instance => shared ?? (shared = new DataManager());

    // Fetches all necessary initial data
    public IEnumerator FetchInitialData(Action callback = null)
    {
        yield return runner.StartCoroutine(FetchCurrentUser());
        yield return runner.StartCoroutine(FetchAllParts());
        yield return runner.StartCoroutine(FetchUserInventory());
        yield return runner.StartCoroutine(FetchUserTeams());
        callback?.Invoke();
    }

    private IEnumerator FetchCurrentUser(Action callback = null)
    {
        var request = BasicGet("/user");
        yield return request.SendWebRequest();
        currentUser = JsonUtils.DeserializeObject<UserInfo>(request.downloadHandler.text);
        callback?.Invoke();
    }

    private IEnumerator FetchAllParts(Action callback = null)
    {
        var request = BasicGet("/parts");
        yield return request.SendWebRequest();
        allParts = JsonUtils.DeserializeArray<PartInfo>(request.downloadHandler.text);
        callback?.Invoke();
    }

    private IEnumerator FetchUserInventory(Action callback = null)
    {
        var request = BasicGet("/inventory");
        yield return request.SendWebRequest();
        inventory = new List<InventoryItem>(JsonUtils.DeserializeArray<InventoryItem>(request.downloadHandler.text));
        callback?.Invoke();
    }

    // Must be called after calling FetchAllParts
    private IEnumerator FetchUserTeams(Action callback = null)
    {
        var botsRequest = BasicGet("/bots");
        var teamsRequest = BasicGet("/teams");

        yield return botsRequest.SendWebRequest();
        yield return teamsRequest.SendWebRequest();

        allBots = JsonUtils.DeserializeArray<BotInfo>(botsRequest.downloadHandler.text);
        userTeams = JsonUtils.DeserializeArray<TeamInfo>(teamsRequest.downloadHandler.text);

        foreach (var team in userTeams)
        {
            yield return team.FetchUserInfo(runner);
        }

        callback?.Invoke();
    }

    public IEnumerator FetchUser(string uid, Action<UserInfo> callback)
    {
        if (uid == currentUser.ID)
        {
            callback.Invoke(currentUser);
        }
        else
        {
            var request = BasicGet("/user/" + uid);
            yield return request.SendWebRequest();
            var user = JsonUtils.DeserializeObject<UserInfo>(request.downloadHandler.text);
            callback.Invoke(user);
        }
    }

    public UserInfo CurrentUser => currentUser;

    public IEnumerator UpdateCurrentUser(Action callback = null)
    {
        var updateBody = JsonUtils.SerializeObject(currentUser);
        var request = BasicPut("/user", updateBody);
        yield return request.SendWebRequest();
        callback?.Invoke();
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

    public IEnumerator UpdateBot(BotInfo bot, Action callback = null)
    {
        var updateBody = JsonUtils.SerializeObject(bot);
        var request = BasicPut("/bots" + bot.ID, updateBody);
        yield return request.SendWebRequest();
        callback?.Invoke();
    }

    public TeamInfo[] UserTeams => userTeams;

    public TeamInfo GetTeam(int tid)
    {
        return userTeams.First(team => team.ID == tid);
    }

    public IEnumerator UpdateTeam(TeamInfo team, Action callback = null)
    {
        var updateBody = JsonUtils.SerializeObject(team);
        var request = BasicPut("/teams" + team.ID, updateBody);
        yield return request.SendWebRequest();
        callback?.Invoke();
    }

    public IEnumerator GetOtherUserTeams(string uid, Action<TeamInfo[]> callback)
    {
        var request = BasicGet("/users/" + uid + "/teams?expandBots=true");
        yield return request.SendWebRequest();
        var teams = JsonUtils.DeserializeArray<TeamInfo>(request.downloadHandler.text);

        foreach (var team in teams)
        {
            yield return team.FetchUserInfo(runner);
        }

        callback.Invoke(teams);
    }

    public IEnumerator GetOtherUserTeam(string uid, int tid, Action<TeamInfo> callback)
    {
        yield return runner.StartCoroutine(GetOtherUserTeams(uid,
            teamInfo => { callback.Invoke(teamInfo.FirstOrDefault(team => team.ID == tid)); }));
    }

}