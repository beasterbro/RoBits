using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    private BotInfo currentBot;
    private List<InventoryItem> userInventory;

    public int teamArrayValue;
    public int botArrayValue;
    

    private bool AddPartToInventory(PartInfo item)
    {
        return DataManager.GetManager().AddItemToUserInventory(item);
    }

    private bool RemovePartFromInventory(PartInfo item)
    {
        return DataManager.GetManager().RemoveItemFromUserInventory(item);
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
        currentBot = DataManager.GetManager().GetUserBotTeams()[teamArrayValue].GetBots()[botArrayValue];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
