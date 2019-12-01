using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class TeamEditorController : MonoBehaviour
{
    
    [SerializeField] private List<TeamEditorUserBot> userBotArray;
    [SerializeField] private GameObject team1parent;
    [SerializeField] private GameObject team2parent;
    [SerializeField] private GameObject team3parent;

    [SerializeField] private RemoveMenu removeMenu;
    [SerializeField] private AddToTeamMenu AddMenu;
    [SerializeField] private AddToTeamSubMenu AddSubMenu;

    private List<TeamBot> team1 = new List<TeamBot>();
    private List<TeamBot> team2 = new List<TeamBot>();
    private List<TeamBot> team3 = new List<TeamBot>();
    
    private List<GameObject> botPreviews = new List<GameObject>();
    private List<BotInfo> userBots;
    private TeamInfo[] userTeams;


    private PartInfo defaultBody ;
    private BotInfo defaultBotInfo  ;
    public event Action<TeamBot> OnLeftClickEvent;
    public event Action<TeamBot> OnRightClickEvent;
    
    // Start is called before the first frame update
    void Start()
    {
        if (!DataManager.Instance.AuthEstablished) DataManager.Instance.EstablishAuth("DEV testUser@gmail.com");
        
           
        StartCoroutine(DataManager.Instance.FetchInitialData(success =>
        {
            if (!success) return;
                
            userBots = new List<BotInfo>(DataManager.Instance.AllBots);


            IEnumerator<BotInfo> userBotEnum = userBots.GetEnumerator();
            userBotEnum.MoveNext();
            foreach (var bot in userBotArray)
            {
                bot.BotInfo = userBotEnum.Current;
                botPreviews.Add(bot.gameObject);
                userBotEnum.MoveNext();
            }
            userBotEnum.Dispose();
            
            userTeams = DataManager.Instance.UserTeams;
            BotPreviewGenerator.BotGenerators = botPreviews;
            BotPreviewGenerator.CreateAllBotImages();
            defaultBody  = DataManager.Instance.GetPart(100);
            defaultBotInfo = new BotInfo(0,"default",0,new List<PartInfo>(),defaultBody,new List<BehaviorInfo>() );
            InstantiateTeams();
        }));
        StopCoroutine(DataManager.Instance.FetchInitialData());     
   
    }


    void InstantiateTeams()
    {
        AddTeamBotsToTeams();
        AddUserTeamBotsToTeamBots();
        RefreshTeamBotPreviews();
        AddActionsToBots();
    }

    private void RefreshTeamBotPreviews()
    {
        var teamBots = team1.Concat(team2).Concat(team3).ToList();
        foreach (var teamBot in teamBots)
        {
            BotPreviewGenerator.CreateBotImage(teamBot.BotInfo,teamBot.gameObject);
        }
    }

    private void AddActionsToBots()
    {
        var teamBots = team1.Concat(team2).Concat(team3).ToList();
        foreach (var teamBot in teamBots)
        {//TODO: Removes bots from a given team, and updates previews, and updates user teams
            teamBot.OnRightClickEvent += ShowRemoveMenu;
            teamBot.OnLeftClickEvent += ShowRemoveMenu;
        }

        foreach (var userBot in userBotArray)
        {
            userBot.OnRightClickEvent += ShowAddMenu;
            userBot.OnLeftClickEvent += ShowAddMenu;
        }
        
    }

    private void ShowAddMenu(TeamEditorUserBot userBot)
    {
        AddMenu.transform.position = MousePosition();
        AddMenu.ShowAddToTeamMenu(userBot.BotInfo);
    }

    public void ShowAddSubMenu(int team)
    {
        AddSubMenu.transform.position = MousePosition();
        AddSubMenu.ShowAddToTeamSubMenu(team);
    }

    public void AddToTeam(int teamSlot)
    {
        //TODO: Make method to update backend info with frontend info
        // DataManager.Instance.UserTeams[team].Bots[teamSlot] = AddMenu.BotInfo;
        if (AddSubMenu.Team == 1)
        {
            team1[teamSlot].BotInfo = AddMenu.BotInfo;
        }
        
        if (AddSubMenu.Team  == 2)
        {
            team2[teamSlot].BotInfo = AddMenu.BotInfo;
        }
        
        if (AddSubMenu.Team  == 3)
        {
            team3[teamSlot].BotInfo = AddMenu.BotInfo;
        }
        CancelAddSubMenu();
        RefreshTeamBotPreviews();
    }

    public void CancelAddMenu()
    {
        AddMenu.CancelAddToTeamMenu();
    }

    public void CancelAddSubMenu()
    {
        AddSubMenu.CancelAddToTeamSubMenu();
        AddMenu.CancelAddToTeamMenu();
    }

    private void ShowRemoveMenu(TeamBot teamBot)
    {
        removeMenu.transform.position = MousePosition();
        removeMenu.ShowRemoveMenu(teamBot);
    }
    
    public void CancelRemoveMenu()
    {
        removeMenu.CancelRemoveMenu();
    }

    public  void Remove()
    {
        RemoveBotFromTeam(removeMenu.TeamBot);
        RefreshTeamBotPreviews();
    }
    void RemoveBotFromTeam(TeamBot teamBot)
    {
        if (userTeams[0]==teamBot.CurrentTeam)
        {
            int i = team1.FindIndex( bot => bot.BotInfo == teamBot.BotInfo);
            team1[i].BotInfo = defaultBotInfo;
        }
        
        if (teamBot.CurrentTeam == userTeams[1])
        {
            int i = team2.FindIndex( bot => bot.BotInfo == teamBot.BotInfo);
            team2[i].BotInfo = defaultBotInfo;
        }
        
        if (teamBot.CurrentTeam == userTeams[2])
        {
            int i = team3.FindIndex( bot => bot.BotInfo == teamBot.BotInfo);
            team3[i].BotInfo = defaultBotInfo;
        }
        removeMenu.gameObject.SetActive(false);
    }
    
    Vector3 MousePosition()
    {
        var v3 = Input.mousePosition;
        v3.z = 1;
        v3 = Camera.main.ScreenToWorldPoint(v3);
        return v3;
    }

    private void AddUserTeamBotsToTeamBots()
    {
        var allTeams = new List<List<TeamBot>>(){team1,team2,team3};
        List<List<TeamBot>>.Enumerator allTeamsEnum = allTeams.GetEnumerator();
        allTeamsEnum.MoveNext();
        
        foreach (var teamInfo in userTeams)
        {
            var currTeamEnum = allTeamsEnum.Current.GetEnumerator();
            currTeamEnum.MoveNext();
            foreach (var bot in teamInfo.Bots)
            {
                currTeamEnum.Current.BotInfo = bot;
                currTeamEnum.Current.CurrentTeam = teamInfo;
                currTeamEnum.MoveNext();
            }

            allTeamsEnum.MoveNext();
        }
        
        allTeamsEnum.Dispose();
    }

    private void AddTeamBotsToTeams()
    {
        foreach (var teamBot in team1parent.GetComponentsInChildren<TeamBot>())
        {
            team1.Add(teamBot);
        }
               
        foreach (var teamBot in team2parent.GetComponentsInChildren<TeamBot>())
        {
            team2.Add(teamBot);
        }
               
        foreach (var teamBot in team3parent.GetComponentsInChildren<TeamBot>())
        {
            team3.Add(teamBot);
        }
    }

    public void UpdateUserTeams()
    {
        TeamInfo[] userTeams = DataManager.Instance.UserTeams;
        TeamInfo currentTeam = userTeams[0];
        TeamInfo teamInfo = new TeamInfo(currentTeam.ID,currentTeam.Name,currentTeam.DateLastMaintained,
            team1.ConvertAll(teamBot => teamBot.BotInfo).ToArray(),currentTeam.Rank,currentTeam.Tier,currentTeam.UserID);
        DataManager.Instance.UpdateTeam(teamInfo, success =>
        {
            if (!success) return;
            
        });
    }
}