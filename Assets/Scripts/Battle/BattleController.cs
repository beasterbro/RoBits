using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    public Transform[] playerLocations;
    public Transform[] enemyLocations;

    [HideInInspector] public TeamInfo team1;

    [HideInInspector] public List<BotController> playerBots = new List<BotController>();
    [HideInInspector] public List<BotController> enemyBots = new List<BotController>();

    async void Start()
    {
        DataManager.GetManager().EstablishAuth("lucaspopp0@gmail.com");
        await DataManager.GetManager().FetchInitialData();

        team1 = DataManager.GetManager().GetTeam(0);

        for (int i = 0; i < team1.GetBots().Length; i++)
        {
            BotInfo botInfo = team1.GetBots()[i];
            GameObject botObject = Instantiate(Resources.Load<GameObject>("Battle/BasicBot"));
            BotController controller = botObject.GetComponent<BotController>();
            if (controller != null) playerBots.Add(controller);
            controller.LoadInfo(botInfo);

            botObject.transform.SetPositionAndRotation(playerLocations[i].transform.position,
                playerLocations[i].transform.rotation);
        }
    }
}