using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    public GameObject team1HUD;
    public GameObject team2HUD;

    private GameObject[] huds = new GameObject[2];

    public Transform[] playerLocations;
    public Transform[] enemyLocations;

    private Transform[][] locations = new Transform[2][];

    private TeamInfo[] teams = new TeamInfo[2];

    private List<BotController>[] bots = new List<BotController>[2];

    async void Start()
    {
        DataManager.GetManager().EstablishAuth("lucaspopp0@gmail.com");
        await DataManager.GetManager().FetchInitialData();

        teams[0] = DataManager.GetManager().GetTeam(0);
        teams[1] = await DataManager.GetManager().GetOtherUserTeam("axs1477", 0);

        huds[0] = team1HUD;
        huds[1] = team2HUD;

        locations[0] = playerLocations;
        locations[1] = enemyLocations;

        LoadTeam(0);
        LoadTeam(1);
    }

    private void LoadTeam(int teamIndex)
    {
        TeamInfo teamInfo = teams[teamIndex];
        bots[teamIndex] = new List<BotController>();

        for (var botIndex = 0; botIndex < teams[teamIndex].GetBots().Length; botIndex++)
        {
            LoadBot(teamIndex, botIndex);
        }
    }

    private void LoadBot(int teamIndex, int botIndex)
    {
        BotInfo botInfo = teams[teamIndex].GetBots()[botIndex];
        GameObject botObject = Instantiate(Resources.Load<GameObject>("Battle/BasicBot"));
        BotController controller = botObject.GetComponent<BotController>();
        if (controller != null) bots[teamIndex].Add(controller);
        controller.LoadInfo(botInfo, teams[teamIndex]);

        botObject.transform.SetPositionAndRotation(locations[teamIndex][botIndex].transform.position,
            locations[teamIndex][botIndex].transform.rotation);

        GameObject statusDisplayObject =
            Instantiate(Resources.Load<GameObject>("Battle/BotStatusDisplay"), huds[teamIndex].transform);
        BotStatusDisplay display = statusDisplayObject.GetComponent<BotStatusDisplay>();
        display.bot = controller;
    }
}