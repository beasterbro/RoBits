using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JsonData;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[AddComponentMenu("Behavior Lab/Behavior Lab Controller")]
public class BehaviorLabController : MonoBehaviour
{

    private static BehaviorLabController currentInstance;

    public Text sidebarTitle;
    public ListController existingTriggersList;
    public ListController newTriggersList;
    public Button addCancelButton;

    public BotInfo currentBot;
    public BehaviorInfo currentBehavior;

    public List<Block> existingBlocks = new List<Block>();
    public TriggerBlock currentTriggerBlock;

    public static BehaviorLabController GetShared()
    {
        if (currentInstance == null) currentInstance = FindObjectOfType<BehaviorLabController>();
        return currentInstance;
    }

    // Start is called before the first frame update
    void Start()
    {
        DataManager.Instance.Latch(this);
        if (!DataManager.Instance.AuthEstablished) DataManager.Instance.BypassAuth("DEV lucaspopp0@gmail.com");
        StartCoroutine(DataManager.Instance.FetchInitialDataIfNecessary(success =>
        {
            if (!success) return;

            BotPreviewController.CreateBotPreviews();

            UpdateCurrentBot(0);

            existingTriggersList.gameObject.SetActive(true);
            newTriggersList.gameObject.SetActive(false);
        }));
    }

    public void UpdateCurrentBot(int id)
    {
        if (currentBot != null)
        {
            SaveCurrentBehavior();
            ClearExistingBlocks();
        }

        currentBot = DataManager.Instance.AllBots[id];
        BotPreviewController.UpdateCurrentPreview(currentBot);

        UpdateBotSpecificBlocks();

        if (currentBot.Behaviors.Count > 0) DisplayBehaviorForTrigger(currentBot.Behaviors[0].Trigger);

        UpdateTriggerLists();
    }

    private void UpdateBotSpecificBlocks()
    {
        // Update which blocks are active based on what parts the current bot has
        BlockSupplier.UpdateActivity();
        // Update shootAt dropdown items
        ShootAtBlock.UpdateDropdownItems();
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
        TriggerInfo trigger;
        foreach (var triggerTuple in TriggerInfo.triggers.Values)
        {
            trigger = triggerTuple.Item1;
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

    private void AddTrigger()
    {
        var selectedItem = EventSystem.current.currentSelectedGameObject.GetComponent<TriggerListItem>();

        if (selectedItem != null && selectedItem.data is TriggerInfo trigger)
        {
            currentBot.Behaviors.Add(new BehaviorInfo(trigger.ID, 0, new[]
            {
                new BlockInfo(0, "Trigger", new Dictionary<string, string> {{"triggerId", trigger.ID.ToString()}}, new[] {-1}, new int[1])
            }));

            ToggleBehaviors();
            UpdateTriggerLists();

            SaveCurrentBehavior();
            ClearExistingBlocks();
            DisplayBehaviorForTrigger(trigger);
        }
    }

    private void UpdateActiveBehavior()
    {
        var selectedItem = EventSystem.current.currentSelectedGameObject.GetComponent<TriggerListItem>();

        if (selectedItem != null && selectedItem.data is TriggerInfo trigger)
        {
            SaveCurrentBehavior();
            ClearExistingBlocks();
            DisplayBehaviorForTrigger(trigger);
        }
    }

    public void AddBlock(Block block)
    {
        existingBlocks.Add(block);
    }

    public void RemoveBlock(Block block)
    {
        existingBlocks.Remove(block);
    }

    public Block GetBlockById(int id) => existingBlocks.FirstOrDefault(block => block.info.ID == id);

    private void ClearExistingBlocks()
    {
        existingBlocks.ForEach(block => { if (block != null) Destroy(block.gameObject); });
        existingBlocks.Clear();
    }

    private void SaveCurrentBehavior()
    {
        if (currentBehavior == null) return;

        var currentIndex = currentBot.Behaviors.FindIndex(behavior => behavior.TriggerId == currentBehavior.TriggerId);

        if (currentIndex == -1) return;
        currentBot.Behaviors[currentIndex] = currentTriggerBlock.BehaviorState();
    }

    private void DisplayBehaviorForTrigger(TriggerInfo trigger)
    {
        currentBehavior = currentBot.Behaviors.FirstOrDefault(behaviorInfo => behaviorInfo.TriggerId == trigger.ID);

        if (currentBehavior != null)
        {
            var behaviorBlocks = currentBehavior.Blocks.Select(block => block != null ? Block.FromInfo(block) : null);

            foreach (var block in behaviorBlocks.Where(block => block != null)) AddBlock(block);

            currentTriggerBlock = GetBlockById(currentBehavior.EntryBlockId) as TriggerBlock;
            if (currentTriggerBlock != null)
            {
                currentTriggerBlock.gameObject.transform.SetPositionAndRotation(new Vector2(-6.3f, 4f), Quaternion.Euler(0, 0, 0));
                StartCoroutine(LetStartThen(currentTriggerBlock.PositionConnections));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log(JsonUtils.SerializeObject(currentTriggerBlock.BehaviorState()));
        }
    }

    public static ICollection<string> CurrentMatchingEquipmentAsResources(PartType type)
    {
        return CurrentMatchingEquipmentAsResources(type, false);
    }

    public static ICollection<string> CurrentMatchingEquipmentAsResources(PartType type, bool uniqueEquipmentOnly)
    {
        ICollection<string> result = uniqueEquipmentOnly ? new HashSet<string>() : new List<string>() as ICollection<string>;
        if (GetShared().currentBot != null)
        {
            foreach (PartInfo part in GetShared().currentBot.Equipment)
            {
                if (type == part.PartType) result.Add(part.ResourceName);
            }
        }
        return result;
    }

    public int NextBlockID()
    {
        if (existingBlocks.Count == 0) return 0;
        return existingBlocks.Max(block => block.info.ID) + 1;
    }

    public void SaveBehaviors()
    {
        SaveCurrentBehavior();
        StartCoroutine(DataManager.Instance.UpdateBot(currentBot));
    }

    public void BackToCustomizeMenu()
    {
        SceneManager.LoadScene(Scenes.Menu);
    }

    private IEnumerator LetStartThen(Action takeAction)
    {
        yield return null;
        takeAction.Invoke();
    }

}