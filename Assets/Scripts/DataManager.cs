using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using JsonData;
using UnityEngine;
using UnityEngine.Networking;

// Collects and manages necessary information that needs to be taken from the backend to the frontend and vice versa.
public class DataManager
{

    private static string baseUrl = "http://robits.us-east-2.elasticbeanstalk.com/api";
    private static DataManager shared;

    private bool initialDataFetched;
    private string bearerToken;
    private MonoBehaviour runner;

    private UserInfo currentUser;
    private PartInfo[] allParts;
    private List<InventoryItem> inventory;

    private BotInfo[] allBots;
    private TeamInfo[] userTeams;

    public static DataManager Instance => shared ?? (shared = new DataManager());

    public UserInfo CurrentUser => currentUser;
    public List<InventoryItem> UserInventory => inventory;
    public PartInfo[] AllParts => allParts;
    public BotInfo[] AllBots => allBots;
    public TeamInfo[] UserTeams => userTeams;
    public bool InitialFetchPerformed => initialDataFetched;

    private DataManager() { }

    // Should be called before using DataManager in a scene. Provides a MonoBehavior object which the
    // DataManager can use to run coroutines
    public void Latch(MonoBehaviour coroutineRunner) => runner = coroutineRunner;

    // Adds the necessary headers to the request
    private UnityWebRequest WrapRequest(UnityWebRequest request)
    {
        request.SetRequestHeader("Authorization", "Bearer " + bearerToken);
        request.SetRequestHeader("Accept", "application/json");
        return request;
    }

    // Convenience methods for building requests to the API
    private UnityWebRequest BasicGet(string endpt) => WrapRequest(UnityWebRequest.Get(baseUrl + endpt));
    private UnityWebRequest BasicPost(string endpt, string content = "") => WrapRequest(UnityWebRequest.Post(baseUrl + endpt, content));
    private UnityWebRequest BasicPut(string endpt, string content = "") => WrapRequest(UnityWebRequest.Put(baseUrl + endpt, content));
    private UnityWebRequest BasicDelete(string endpt) => WrapRequest(UnityWebRequest.Delete(baseUrl + endpt));

    // Adds the auth header to the HTTP client
    public void EstablishAuth(string token)
    {
        bearerToken = token;
    }

    public IEnumerator FetchInitialData(Action callback = null)
    {
        yield return runner.StartCoroutine(FetchCurrentUser());
        yield return runner.StartCoroutine(FetchAllParts());
        if (currentUser != null && allParts != null)
        {
            yield return runner.StartCoroutine(FetchUserInventory());
            yield return runner.StartCoroutine(FetchUserTeams());
        }

        initialDataFetched = true;
        callback?.Invoke();
    }

    private IEnumerator FetchCurrentUser(Action callback = null)
    {
        var request = BasicGet("/user");
        yield return request.SendWebRequest();

        if (request.EncounteredError()) Debug.LogError(request.GetError());
        else currentUser = JsonUtils.DeserializeObject<UserInfo>(request.downloadHandler.text);

        callback?.Invoke();
    }

    private IEnumerator FetchAllParts(Action callback = null)
    {
        var request = BasicGet("/parts");
        yield return request.SendWebRequest();

        if (request.EncounteredError()) Debug.LogError(request.GetError());
        else allParts = JsonUtils.DeserializeArray<PartInfo>(request.downloadHandler.text);

        callback?.Invoke();
    }

    private IEnumerator FetchUserInventory(Action callback = null)
    {
        var request = BasicGet("/inventory");
        yield return request.SendWebRequest();

        if (request.EncounteredError()) Debug.LogError(request.GetError());
        else inventory = new List<InventoryItem>(JsonUtils.DeserializeArray<InventoryItem>(request.downloadHandler.text));

        callback?.Invoke();
    }

    // Must be called after calling FetchAllParts
    private IEnumerator FetchUserTeams(Action callback = null)
    {
        var botsRequest = BasicGet("/bots");
        var teamsRequest = BasicGet("/teams");

        yield return botsRequest.SendWebRequest();
        yield return teamsRequest.SendWebRequest();

        if (botsRequest.EncounteredError()) Debug.LogError(botsRequest.GetError());
        if (teamsRequest.EncounteredError()) Debug.LogError(teamsRequest.GetError());

        if (!botsRequest.EncounteredError() && !teamsRequest.EncounteredError())
        {
            allBots = JsonUtils.DeserializeArray<BotInfo>(botsRequest.downloadHandler.text);
            userTeams = JsonUtils.DeserializeArray<TeamInfo>(teamsRequest.downloadHandler.text);

            foreach (var team in userTeams)
            {
                yield return team.FetchUserInfo(runner);
            }
        }

        callback?.Invoke();
    }

    public IEnumerator FetchUser(string uid, Action<UserInfo> callback)
    {
        if (uid == currentUser.ID) callback.Invoke(currentUser);
        else
        {
            var request = BasicGet("/user/" + uid);
            yield return request.SendWebRequest();

            if (request.EncounteredError())
            {
                Debug.LogError(request.GetError());
                callback.Invoke(null);
            }
            else callback.Invoke(JsonUtils.DeserializeObject<UserInfo>(request.downloadHandler.text));
        }
    }

    public IEnumerator UpdateCurrentUser(Action callback = null)
    {
        var updateBody = JsonUtils.SerializeObject(currentUser);
        var request = BasicPut("/user", updateBody);
        yield return request.SendWebRequest();
        if (request.EncounteredError()) Debug.LogError(request.GetError());
        callback?.Invoke();
    }

    public PartInfo GetPart(int pid) => allParts.FirstOrDefault(part => part.ID == pid);

    public IEnumerator SellPart(PartInfo item, Action<bool> callback)
    {
        var request = BasicPost("/inventory/sell/" + item.ID);
        yield return request.SendWebRequest();

        if (request.EncounteredError())
        {
            Debug.LogError(request.GetError());
            callback.Invoke(false);
        }
        else
        {
            yield return runner.StartCoroutine(FetchCurrentUser());
            yield return runner.StartCoroutine(FetchUserInventory());
            callback.Invoke(true);
        }
    }

    public IEnumerator PurchasePart(PartInfo item, Action<bool> callback)
    {
        var request = BasicPost("/inventory/purchase/" + item.ID);
        yield return request.SendWebRequest();

        if (request.EncounteredError())
        {
            Debug.LogError(request.GetError());
            callback.Invoke(false);
        }
        else
        {
            yield return runner.StartCoroutine(FetchCurrentUser());
            yield return runner.StartCoroutine(FetchUserInventory());
            callback.Invoke(true);
        }
    }

    public BotInfo GetBot(int bid) => allBots.FirstOrDefault(bot => bot.ID == bid);

    public IEnumerator UpdateBot(BotInfo bot, Action callback = null)
    {
        var updateBody = JsonUtils.SerializeObject(bot);
        var request = BasicPut("/bots" + bot.ID, updateBody);
        yield return request.SendWebRequest();
        if (request.EncounteredError()) Debug.LogError(request.GetError());
        callback?.Invoke();
    }

    public TeamInfo GetTeam(int tid) => userTeams.FirstOrDefault(team => team.ID == tid);

    public IEnumerator UpdateTeam(TeamInfo team, Action callback = null)
    {
        var updateBody = JsonUtils.SerializeObject(team);
        var request = BasicPut("/teams" + team.ID, updateBody);
        yield return request.SendWebRequest();
        if (request.EncounteredError()) Debug.LogError(request.GetError());
        callback?.Invoke();
    }

    public IEnumerator GetOtherUserTeams(string uid, Action<TeamInfo[]> callback)
    {
        var request = BasicGet("/users/" + uid + "/teams?expandBots=true");
        yield return request.SendWebRequest();

        if (request.EncounteredError())
        {
            Debug.LogError(request.GetError());
            callback.Invoke(null);
        }
        else
        {
            var teams = JsonUtils.DeserializeArray<TeamInfo>(request.downloadHandler.text);

            foreach (var team in teams)
            {
                yield return team.FetchUserInfo(runner);
            }

            callback.Invoke(teams);
        }
    }

    public IEnumerator GetOtherUserTeam(string uid, int tid, Action<TeamInfo> callback)
    {
        yield return runner.StartCoroutine(GetOtherUserTeams(uid, teamInfo => { callback.Invoke(teamInfo.FirstOrDefault(team => team.ID == tid)); }));
    }

}