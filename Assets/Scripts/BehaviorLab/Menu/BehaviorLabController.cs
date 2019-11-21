using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Behavior Lab/Behavior Lab Controller")]
public class BehaviorLabController : MonoBehaviour
{
    public static BotInfo currentBot;
    public static TriggerBlock Trigger => TriggerSpawner.Trigger;

    // Start is called before the first frame update
    async void Start()
    {
        int currentBotId;
        if (DataManager.Instance.CurrentUser == null)
        {
            DataManager.Instance.EstablishAuth("DEV lucaspopp0@gmail.com");
            await DataManager.Instance.FetchInitialData();
            currentBotId = DataManager.Instance.AllBots[0].ID;
        }
        else
        {
            currentBotId = 0;
            //currentBotId = InventoryController.CurrentBot.ID;
        }
        UpdateCurrentBot(currentBotId);
        SensorBarController.Instance.Load(Sensors.SensorTypeHelper.KnownTypes);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            PrintBehavior();
        }
    }

    private void PrintBehavior()
    {
        Debug.Log(Trigger.BehaviorState());
    }

    public static void UpdateCurrentBot(int bid)
    {
        currentBot = DataManager.Instance.GetBot(bid);
    }

    public static void UpdateTrigger(int id)
    {
        // TODO: Update displayed trigger as follows:
        // Do something with current trigger/behavior info
        // Remove all (connected) blocks from trigger block
        // Update id of trigger block
        Trigger.UpdateTrigger(id);
        // Load in existing behavior from user's bot
    }
}
