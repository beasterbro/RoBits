using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Extensions;
using JsonData;
using UnityEngine;
using UnityEngine.Networking;

// Collects and manages necessary information that needs to be taken from the backend to the frontend and vice versa.
public class DataManager
{

    private struct AuthResponse
    {

        public string name, email, token;

    }

    private static string baseUrl = "http://robits.us-east-2.elasticbeanstalk.com/api";
    private static DataManager shared;

    private bool authEstablished;
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
    public bool AuthEstablished => authEstablished;

    private DataManager() { }

    // Should be called before using DataManager in a scene. Provides a MonoBehavior object which the
    // DataManager can use to run coroutines
    public void Latch(MonoBehaviour coroutineRunner) => runner = coroutineRunner;

    // Adds the necessary headers to the request
    private UnityWebRequest WrapRequest(UnityWebRequest request, bool ignoreAuth = false)
    {
        if (!ignoreAuth && authEstablished && bearerToken != null) request.SetRequestHeader("Authorization", "Bearer " + bearerToken);
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Accept", "application/json");
        return request;
    }

    // Convenience methods for building requests to the API
    private UnityWebRequest BasicGet(string endpt, bool ignoreAuth = false)
        => WrapRequest(UnityWebRequest.Get(baseUrl + endpt), ignoreAuth);
    
    private UnityWebRequest BasicPost(string endpt, string content = "", bool ignoreAuth = false)
    {
        var request = UnityWebRequest.Post(baseUrl + endpt, content);
        
        if (!string.IsNullOrEmpty(content))
        {
            request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(content));
            request.uploadHandler.contentType = "application/json";
        }
        
        return WrapRequest(request, ignoreAuth);
    }

    private UnityWebRequest BasicPut(string endpt, string content = "", bool ignoreAuth = false)
        => WrapRequest(UnityWebRequest.Put(baseUrl + endpt, content), ignoreAuth);

    // Provides a simple callback handler, passing a boolean representing success/failure
    private static void SimpleCallback(UnityWebRequest request, Action action, Action<bool> callback = null)
    {
        if (request.EncounteredError())
        {
            Debug.LogError(request.GetError());
            callback?.Invoke(false);
        }
        else
        {
            try
            {
                action?.Invoke();
                callback?.Invoke(true);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                callback?.Invoke(false);
            }
        }
    }

    // Provides a simple callback handler, allowing for separate success/failure responses
    private static void SimpleCallback(UnityWebRequest request, Action action, Action success, Action failure)
    {
        SimpleCallback(request, action, result =>
        {
            if (result) success.Invoke();
            else failure.Invoke();
        });
    }

    // Exchange a Google ID token for a bearer token
    public IEnumerator EstablishAuth(string googleToken, Action<bool, string> callback = null)
    {
        authEstablished = false;

        var request = BasicPost("/verify?id_token=" + googleToken);
        yield return request.SendWebRequest();

        string email = null;
        SimpleCallback(request, () =>
        {
            var response = JsonUtils.DeserializeObject<AuthResponse>(request.downloadHandler.text);
            bearerToken = response.token;
            email = response.email;
            authEstablished = true;
        }, success =>
        {
            callback?.Invoke(success, email);
        });
    }

    // Adds the auth header to the HTTP client
    public void BypassAuth(string token)
    {
        bearerToken = token;
        authEstablished = true;
    }

    public IEnumerator EmailIsTaken(string email, Action<bool, bool> callback)
    {
        var request = BasicGet("/user/exists?email=" + email, ignoreAuth: true);
        yield return request.SendWebRequest();

        bool taken = false;
        SimpleCallback(request, () =>
        {
            taken = JsonUtils.DeserializeObject<bool>(request.downloadHandler.text);
        }, success =>
        {
            callback.Invoke(success, taken);
        });
    }

    public IEnumerator UsernameIsTaken(string username, Action<bool> callback)
    {
        var request = BasicGet("/user/exists?username=" + username, ignoreAuth: true);
        yield return request.SendWebRequest();

        bool taken = false;
        SimpleCallback(request, () =>
        {
            taken = JsonUtils.DeserializeObject<bool>(request.downloadHandler.text);
        }, success =>
        {
            callback.Invoke(!success || taken);
        });
    }

    public IEnumerator CreateUser(string username, string email, Action<bool> callback)
    {
        var userCreateRequest = new Dictionary<string, string>
        {
            {"uid", username},
            {"username", username},
            {"email", email}
        };
        var createContent = JsonUtils.SerializeObject(userCreateRequest);
        
        var request = BasicPost("/user", createContent, ignoreAuth: true);
        yield return request.SendWebRequest();
        SimpleCallback(request, null, callback);
    }

    public IEnumerator FetchInitialDataIfNecessary(Action<bool> callback = null)
    {
        if (initialDataFetched)
        {
            yield return null;
            callback?.Invoke(true);
        }
        else yield return runner.StartCoroutine(FetchInitialData(callback));
    }

    public IEnumerator FetchInitialData(Action<bool> callback = null)
    {
        bool userFetched = false, partsFetched = false, inventoryFetched = false, teamsFetched = false;
        yield return runner.StartCoroutine(FetchCurrentUser(success => userFetched = success));
        yield return runner.StartCoroutine(FetchAllParts(success => partsFetched = success));

        if (partsFetched) TriggerInfo.LoadTriggers();

        if (userFetched && partsFetched)
        {
            yield return runner.StartCoroutine(FetchUserInventory(success => inventoryFetched = success));
            yield return runner.StartCoroutine(FetchUserTeams(success => teamsFetched = success));
        }

        if (userFetched && partsFetched && inventoryFetched && teamsFetched)
        {
            initialDataFetched = true;
            callback?.Invoke(true);
        }
        else callback?.Invoke(false);
    }

    public IEnumerator FetchCurrentUser(Action<bool> callback = null)
    {
        var request = BasicGet("/user");
        yield return request.SendWebRequest();

        SimpleCallback(request, () =>
        {
            currentUser = JsonUtils.DeserializeObject<UserInfo>(request.downloadHandler.text);
        }, callback);
    }

    public IEnumerator FetchAllParts(Action<bool> callback = null)
    {
        var request = BasicGet("/parts");
        yield return request.SendWebRequest();

        SimpleCallback(request, () =>
        {
            allParts = JsonUtils.DeserializeArray<PartInfo>(request.downloadHandler.text);
        }, callback);
    }

    public IEnumerator FetchUserInventory(Action<bool> callback = null)
    {
        var request = BasicGet("/inventory");
        yield return request.SendWebRequest();

        SimpleCallback(request, () =>
        {
            inventory = new List<InventoryItem>(JsonUtils.DeserializeArray<InventoryItem>(request.downloadHandler.text));
        }, callback);
    }

    public IEnumerator UpdateUserInventory(InventoryItem[] updatedInventory, Action<bool> callback = null)
    {
        var updateBody = JsonUtils.SerializeObject(updatedInventory);
        var request = BasicPut("/inventory", updateBody);
        yield return request.SendWebRequest();
        
        SimpleCallback(request, () =>
        {
            inventory = new List<InventoryItem>(updatedInventory);
        }, callback);
    }

    // Must be called after calling FetchAllParts
    private IEnumerator FetchUserTeams(Action<bool> callback = null)
    {
        var botsRequest = BasicGet("/bots");
        var teamsRequest = BasicGet("/teams");

        yield return botsRequest.SendWebRequest();
        yield return teamsRequest.SendWebRequest();

        bool botsLoaded = false, teamsLoaded = false;

        SimpleCallback(botsRequest, () =>
        {
            allBots = JsonUtils.DeserializeArray<BotInfo>(botsRequest.downloadHandler.text);
        }, success => botsLoaded = success);

        if (botsLoaded)
        {
            SimpleCallback(teamsRequest, () =>
            {
                userTeams = JsonUtils.DeserializeArray<TeamInfo>(teamsRequest.downloadHandler.text);
                foreach (var team in userTeams) team.SetUserInfo(currentUser);
            }, success => teamsLoaded = success);
        }

        callback?.Invoke(teamsLoaded);
    }

    public IEnumerator FetchUser(string uid, Action<bool, UserInfo> callback)
    {
        if (currentUser != null && uid == currentUser.ID) callback.Invoke(true, currentUser);
        else
        {
            var request = BasicGet("/user/" + uid);
            yield return request.SendWebRequest();

            UserInfo userResponse = null;

            SimpleCallback(request, () =>
            {
                userResponse = JsonUtils.DeserializeObject<UserInfo>(request.downloadHandler.text);
            }, success => callback.Invoke(success, userResponse));
        }
    }

    public IEnumerator FetchRandomOpponent(Action<bool, UserInfo> callback)
    {
        var request = BasicGet("/opponent");
        yield return request.SendWebRequest();
        
        UserInfo userResponse = null;

        SimpleCallback(request, () =>
        {
            userResponse = JsonUtils.DeserializeObject<UserInfo>(request.downloadHandler.text);
        }, success => callback.Invoke(success, userResponse));
    }

    public IEnumerator SearchUser(string username, Action<bool, UserInfo> callback)
    {
        if (currentUser != null && username == currentUser.Username) callback.Invoke(true, currentUser);
        else
        {
            var request = BasicGet("/user/search/" + username);
            yield return request.SendWebRequest();

            UserInfo userResponse = null;

            SimpleCallback(request, () =>
            {
                userResponse = JsonUtils.DeserializeObject<UserInfo>(request.downloadHandler.text);
            }, success => callback.Invoke(success, userResponse));
        }
    }

    public IEnumerator UpdateCurrentUser(Action<bool> callback = null)
    {
        var updateBody = JsonUtils.SerializeObject(currentUser);
        var request = BasicPut("/user", updateBody);
        yield return request.SendWebRequest();
        SimpleCallback(request, null, callback);
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
            bool currentUserFetched = false, inventoryFetched = false;

            yield return runner.StartCoroutine(FetchCurrentUser(success => currentUserFetched = success));
            yield return runner.StartCoroutine(FetchUserInventory(success => inventoryFetched = success));
            callback.Invoke(currentUserFetched && inventoryFetched);
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
            bool currentUserFetched = false, inventoryFetched = false;

            yield return runner.StartCoroutine(FetchCurrentUser(successs => currentUserFetched = successs));
            yield return runner.StartCoroutine(FetchUserInventory(success => inventoryFetched = success));
            callback.Invoke(currentUserFetched && inventoryFetched);
        }
    }

    public BotInfo GetBot(int bid) => allBots.FirstOrDefault(bot => bot.ID == bid);

    public IEnumerator UpdateBot(BotInfo bot, Action<bool> callback = null)
    {
        var updateBody = JsonUtils.SerializeObject(bot);
        var request = BasicPut("/bots/" + bot.ID, updateBody);
        yield return request.SendWebRequest();

        SimpleCallback(request, null, callback);
    }

    public TeamInfo GetTeam(int tid) => userTeams.FirstOrDefault(team => team.ID == tid);

    public IEnumerator UpdateTeam(TeamInfo team, Action<bool> callback = null)
    {
        var updateBody = JsonUtils.SerializeObject(team);
        var request = BasicPut("/teams/" + team.ID, updateBody);
        yield return request.SendWebRequest();

        var botUpdateFailed = false;
        if (!request.EncounteredError())
        {
            foreach (var bot in team.Bots)
            {
                yield return runner.StartCoroutine(UpdateBot(bot, success =>
                {
                    if (!success) botUpdateFailed = true;
                }));
            }
        }

        if (botUpdateFailed) callback?.Invoke(false);
        else SimpleCallback(request, null, callback);
    }

    public IEnumerator GetOtherUserTeams(string uid, Action<bool, TeamInfo[]> callback)
    {
        UserInfo otherUser = null;

        yield return FetchUser(uid, (success, otherInfo) =>
        {
            if (success) otherUser = otherInfo;
        });

        if (otherUser == null) callback.Invoke(false, null);
        else
        {
            var teamsRequest = BasicGet("/users/" + uid + "/teams?expandBots=true");
            yield return teamsRequest.SendWebRequest();

            TeamInfo[] otherTeams = null;

            SimpleCallback(teamsRequest, () =>
            {
                otherTeams = JsonUtils.DeserializeArray<TeamInfo>(teamsRequest.downloadHandler.text);
            }, () =>
            {
                foreach (var team in otherTeams) team.SetUserInfo(otherUser);
                callback.Invoke(true, otherTeams);
            }, () => callback.Invoke(false, null));
        }
    }

    public IEnumerator GetOtherUserTeam(string uid, int tid, Action<bool, TeamInfo> callback)
    {
        yield return runner.StartCoroutine(GetOtherUserTeams(uid, (success, teams) =>
        {
            if (success)
            {
                var team = teams.FirstOrDefault(t => t.ID == tid);
                if (team == null)
                {
                    Debug.LogError("Unable to find team with ID " + tid);
                    callback.Invoke(false, null);
                }
                else
                {
                    callback.Invoke(true, team);
                }
            }
            else callback.Invoke(false, null);
        }));
    }

}