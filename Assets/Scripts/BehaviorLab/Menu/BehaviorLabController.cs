using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JsonData;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[AddComponentMenu("Behavior Lab/Behavior Lab Controller")]
public class BehaviorLabController : MonoBehaviour
{

    public Text sidebarTitle;
    public ListController existingTriggersList;
    public ListController newTriggersList;
    public Button addCancelButton;

    public BotInfo currentBot;
    public TriggerInfo currentTrigger;

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

            UpdateTriggerLists();
            existingTriggersList.gameObject.SetActive(true);
            newTriggersList.gameObject.SetActive(false);
        }));
    }

    public void ToggleBehaviors()
    {
        if (existingTriggersList.gameObject.activeInHierarchy)
        {
            sidebarTitle.text = "ADD NEW TRIGGER";
            existingTriggersList.gameObject.SetActive(false);
            newTriggersList.gameObject.SetActive(true);
            addCancelButton.gameObject.GetComponentInChildren<Text>().text = "CANCEL";
        }
        else
        {
            sidebarTitle.text = "BEHAVIORS";
            existingTriggersList.gameObject.SetActive(true);
            newTriggersList.gameObject.SetActive(false);
            addCancelButton.gameObject.GetComponentInChildren<Text>().text = "ADD";
        }
    }

    private void UpdateTriggerLists()
    {
        existingTriggersList.LoadData(currentBot.Behaviors.Select(behavior => behavior.Trigger));
        existingTriggersList.GenerateItems();

        foreach (var item in existingTriggersList.Items)
        {
            var button = item.GetComponent<Button>();
            if (button != null) button.onClick.AddListener(UpdateActiveBehavior);
        }

        newTriggersList.LoadData(GenerateNewTriggersData());
        newTriggersList.GenerateItems();

        foreach (var item in newTriggersList.Items)
        {
            var button = item.GetComponent<Button>();
            if (button != null) button.onClick.AddListener(AddTrigger);
        }
    }

    private Dictionary<object, IEnumerable<object>> GenerateNewTriggersData()
    {
        var triggersData = new Dictionary<string, List<TriggerInfo>>();

        foreach (var trigger in TriggerInfo.triggers.Values)
        {
            if (currentBot.Behaviors.Exists(behavior => behavior.Trigger.Equals(trigger))) continue;

            var heading = trigger.Sensor == null ? "Basic" : trigger.Sensor.Name;
            if (!triggersData.ContainsKey(heading)) triggersData[heading] = new List<TriggerInfo>();
            triggersData[heading].Add(trigger);
        }

        var finalData = new Dictionary<object, IEnumerable<object>>();
        foreach (var element in triggersData)
        {
            finalData[element.Key] = element.Value.ToArray();
        }

        return finalData;
    }

    public void AddTrigger()
    {
        var selectedItem = EventSystem.current.currentSelectedGameObject.GetComponent<TriggerListItem>();

        if (selectedItem != null && selectedItem.data is TriggerInfo trigger)
        {
            currentBot.Behaviors.Add(new BehaviorInfo(trigger.ID, 0, new BlockInfo[0]));
        }
    }

    public void UpdateActiveBehavior()
    {
        var selectedItem = EventSystem.current.currentSelectedGameObject.GetComponent<TriggerListItem>();

        if (selectedItem != null && selectedItem.data is TriggerInfo trigger)
        {
            DisplayBehaviorForTrigger(trigger);
        }
    }

    private void DisplayBehaviorForTrigger(TriggerInfo trigger)
    {
        currentTrigger = trigger;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log(JsonUtils.SerializeObject(Trigger.BehaviorState()));
        }
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