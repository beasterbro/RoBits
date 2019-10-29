using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

    async void Start()
    {
        winner = -1;
        winnerText.text = "Loading...";

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

        SetAllBotsEnabled(false);

        allLoaded = true;
        winnerText.text = "START!";
        await Task.Delay(1000);
        winnerText.gameObject.SetActive(false);
        SetAllBotsEnabled(true);
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
            winnerText.text = "Winner: " + teams[winner].GetUserID();
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