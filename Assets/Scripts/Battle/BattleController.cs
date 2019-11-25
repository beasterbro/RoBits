using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BattleController : MonoBehaviour
{

    private static BattleController currentInstance;

    public GameObject team1HUD;
    public GameObject team2HUD;
    public Text winnerText;

    private GameObject[] huds = new GameObject[2];

    public Transform[] playerLocations;
    public Transform[] enemyLocations;

    private Transform[][] locations = new Transform[2][];

    private TeamInfo[] teams = new TeamInfo[2];
    private int winner;

    private List<BotController>[] bots = new List<BotController>[2];

    private bool allLoaded = false;

    public static BattleController GetShared()
    {
        if (currentInstance == null) currentInstance = FindObjectOfType<BattleController>();

        return currentInstance;
    }

    void Start()
    {
        winner = -1;
        winnerText.text = "Loading...";

        DataManager.Instance.Latch(this);
        if (!DataManager.Instance.InitialFetchPerformed) DataManager.Instance.EstablishAuth("DEV lucaspopp0@gmail.com");

        StartCoroutine(DataManager.Instance.FetchInitialDataIfNecessary(success =>
        {
            if (!success) return;

            huds = new[] {team1HUD, team2HUD};
            locations = new[] {playerLocations, enemyLocations};
            teams[0] = DataManager.Instance.GetTeam(0);

            StartCoroutine(DataManager.Instance.GetOtherUserTeam("axs1477", 0, (success2, enemyTeam) =>
            {
                if (!success2) return;

                teams[1] = enemyTeam;
                LoadTeam(0);
                LoadTeam(1);

                SetAllBotsEnabled(false);

                StartCoroutine(BeginBattle());
            }));
        }));
    }

    private IEnumerator BeginBattle()
    {
        allLoaded = true;
        winnerText.text = "START!";
        yield return new WaitForSeconds(1);
        winnerText.gameObject.SetActive(false);
        SetAllBotsEnabled(true);
    }

    private void LoadTeam(int teamIndex)
    {
        TeamInfo teamInfo = teams[teamIndex];
        bots[teamIndex] = new List<BotController>();

        for (var botIndex = 0; botIndex < teams[teamIndex].Bots.Length; botIndex++)
        {
            LoadBot(teamIndex, botIndex);
        }
    }

    private void LoadBot(int teamIndex, int botIndex)
    {
        BotInfo botInfo = teams[teamIndex].Bots[botIndex];
        GameObject botObject = Instantiate(Resources.Load<GameObject>("Battle/BasicBot"));
        BotController controller = botObject.GetComponent<BotController>();
        if (controller != null) bots[teamIndex].Add(controller);
        controller.LoadInfo(botInfo, teams[teamIndex]);

        botObject.transform.SetPositionAndRotation(locations[teamIndex][botIndex].transform.position,
            locations[teamIndex][botIndex].transform.rotation);

        GameObject statusDisplayObject =
            Instantiate(Resources.Load<GameObject>("Battle/BotStatusDisplay"), huds[teamIndex].transform);
        BotStatusDisplay display = statusDisplayObject.GetComponent<BotStatusDisplay>();
        display.LoadBot(controller);
    }

    private void Update()
    {
        if (!allLoaded) return;

        for (int i = 0; i <= 1; i++)
        {
            if (!bots[i].All(bot => bot.isDead)) continue;

            winner = 1 - i;
            break;
        }

        if (winner != -1)
        {
            SetAllBotsEnabled(false);
            winnerText.text = "Winner: " + teams[winner].UserID;
            winnerText.gameObject.SetActive(true);
        }
    }

    private void SetAllBotsEnabled(bool enabled)
    {
        foreach (var team in bots)
            team.ForEach(bot => bot.SetEnabled(enabled));
    }

    public IEnumerable<BotController> GetAllBots()
    {
        return bots[0].Concat(bots[1]);
    }

}