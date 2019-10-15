using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour , IHasChanged
{
    private BotInfo currentBot;
    private List<InventoryItem> userInventory;

    [SerializeField] private Transform slots;
    [SerializeField] public Text inventoryText;
    
    public int teamArrayValue;
    public int botArrayValue;
    public BotInfo activeBot;

    private bool AddPartToInventory(PartInfo item)
    {
        return DataManager.GetManager().AddItemToUserInventory(item);
    }

    private bool RemovePartFromInventory(PartInfo item)
    {
        return DataManager.GetManager().RemoveItemFromUserInventory(item);
    }

    public void setActiveBot(BotInfo bot)
    {
        activeBot = bot;
    }

    /**
     * Precondition: the part is currently in the user's inventory
     * Postcondition: the part is on the current bot;
     */
    private bool AddPartToBot(PartInfo part)
    {//TODO: properly implement the conversion from Part to InventoryItem
        InventoryItem itemToRemove = new InventoryItem(part,1);
        userInventory.Remove(itemToRemove);
        return true;
    }

    /**
     * Precondition: the part is currently on the bot
     * Postcondition: the part is no longer on the bot and is added to the user's inventory
     */
    private bool RemovePartFromBot(PartInfo part)
    {
        currentBot.RemovePart(part);
        userInventory.Add(new InventoryItem(part,1));
        return true;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        HasChanged();
        currentBot = DataManager.GetManager().GetUserBotTeams()[teamArrayValue].GetBots()[botArrayValue];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HasChanged()
    {
        System.Text.StringBuilder builder = new System.Text.StringBuilder();
        builder.Append("  -  ");
        foreach (Transform slotTransform in slots)
        {
            GameObject item = slotTransform.GetComponent<SlotController>().item;
            if (item)
            {
                builder.Append(item.name);
                builder.Append("  -  ");
            } 
        }

        inventoryText.text = builder.ToString();
    }
}

namespace UnityEngine.EventSystems 
{
    public interface IHasChanged : IEventSystemHandler
    {
        void HasChanged();
    }

}