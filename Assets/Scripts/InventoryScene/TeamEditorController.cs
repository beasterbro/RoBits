using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class TeamEditorController : MonoBehaviour
{
    [SerializeField] private List<GameObject> botPreviews;
    [SerializeField] private GameObject team1parent;
    [SerializeField] private GameObject team2parent;
    [SerializeField] private GameObject team3parent;

    private List<TeamBot> team1;
    private List<TeamBot> team2;
    private List<TeamBot> team3;
    private List<BotInfo> userBots;

    public event Action<TeamBot> OnLeftClickEvent;
    public event Action<TeamBot> OnRightClickEvent;
    
    // Start is called before the first frame update
    async void Start()
    {
        if (!DataManager.Instance.InitialFetchPerformed)
        {
            DataManager.Instance.EstablishAuth("DEV testUser@gmail.com");
            DataManager.Instance.FetchInitialData(() =>
            {
                userBots = new List<BotInfo>(DataManager.Instance.AllBots);

                BotPreviewGenerator.BotGenerators = botPreviews;
                BotPreviewGenerator.CreateAllBotImages();
                InstantiateTeams();
            });
        }
        
    }

    void InstantiateTeams()
    {
        AddTeamBotsToTeams();
        AddUserBotsToTeamBots();
        AddImagesToTeamBots();
        AddActionsToBots();
    }

    private void AddImagesToTeamBots()
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
          //  teamBot.OnRightClickEvent += RemoveFromTeam;
        }
        
    }

    private void AddUserBotsToTeamBots()
    {
        for (int i = 0; i < userBots.Count; i++)
        {
            if (0 <= i && i <= 2)
            {
                team1[i].BotInfo = userBots[i];
                BotPreviewGenerator.CreateBotImage(team1[i].BotInfo,team1[i].gameObject);
            }
            if (3 <= i && i <= 5)
            {
                team2[i-3].BotInfo = userBots[i];
                BotPreviewGenerator.CreateBotImage(team2[i-3].BotInfo,team2[i-3].gameObject);
            }
            if (6 <= i && i <= 8)
            {
                team1[i-6].BotInfo = userBots[i];
                BotPreviewGenerator.CreateBotImage(team3[i-6].BotInfo,team3[i-6].gameObject);
            }
        }
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
}