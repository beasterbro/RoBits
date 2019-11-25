using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Behavior Lab/Behavior Lab Controller")]
public class BehaviorLabController : MonoBehaviour
{
    public static BotInfo currentBot;
    public static TriggerBlock Trigger => TriggerSpawner.Trigger;

    // Start is called before the first frame update
    void Start()
    {
        DataManager.Instance.Latch(this);
        if (!DataManager.Instance.InitialFetchPerformed) DataManager.Instance.EstablishAuth("DEV lucaspopp0@gmail.com");
        StartCoroutine(DataManager.Instance.FetchInitialDataIfNecessary(success =>
        {
            if (!success) return;

            currentBot = DataManager.Instance.AllBots[0];
            StartCoroutine(DelayedLoad());
        }));
    }

    private IEnumerator DelayedLoad()
    {
        yield return null;
        SensorBarController.Instance.Load(SensorBarController.CurrentBotSensors());
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
