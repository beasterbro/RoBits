using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Image = UnityEngine.Experimental.UIElements.Image;

public class InventoryController : MonoBehaviour , IHasChanged
{
    private static List<BotInfo> userBots;
    public static BotInfo currentBot;
    private List<InventoryItem> userInventory;
    public List<Image> itemImages;

    [SerializeField] private Transform slots;
    [SerializeField] private Text inventoryText;
    [SerializeField] private Text testText;
 
    public BotInfo activeBot;

    private static Dictionary<String,double> attributes1 = new Dictionary<string, double>
    {
        {"DMG",23.0},
        {"DIST",12}
    };
       
    private static Dictionary<String,double> attributes2 = new Dictionary<string, double>
    {
        {"DMG",23.0},
        {"DIST",12}
    };
       
    private static Dictionary<String,string> settings = new Dictionary<string, string>
    {
        {"DARK","yes"},
        {"LOUD","no"}
    };
       
    private static Dictionary<String,double> bodySpec = new Dictionary<string, double>
    {
        {"THICC",11},
        {"SANIC",101}
    };
       
       
    private static PartInfo part1 = new PartInfo(0, "0", "first part", PartType.Weapon, 1, 1, true, attributes1);
    private static PartInfo part2 = new PartInfo(1, "1", "second part", PartType.Weapon, 2, 2, true, attributes2);

    private static List<PartInfo> allParts = new List<PartInfo>(new PartInfo[]{part1,part2});
        
    private static PartInfo body = new PartInfo(2, "body", "thrid part", PartType.BodyType, 2, 2, false, bodySpec);

    private UserInfo _userInfo = new UserInfo("testUser","ass@ass.com","tester101",100,200,true,settings);
       
    private static BotInfo bot0 = new BotInfo(0,"bot0",0,allParts,body);
    private static BotInfo bot1 = new BotInfo(1,"bot1",1,allParts,body);
    private static BotInfo bot2 = new BotInfo(2,"bot2",2,allParts,body);

    private static BotInfo[] botTeam = new[] {bot0, bot1, bot2};
    
    public static bool PurchasePart(PartInfo item)
    {
        return DataManager.instance().PurchasePart(item);
    }

    private bool SellPart(PartInfo item)
    {
        return DataManager.instance().SellPart(item);
    }

    public void SetActiveBot(int botValue)
    {
        //for testing purposes of setting the actively edited bot
        testText.text = "" + botValue;
        // currentBot = DataManager.instance().GetAllBots()[botValue];
        currentBot = botTeam[botValue];
        testText.text = currentBot.GetName();
        
        //TODO: Remove currently equipped parts from inventory
        //TODO: Show currently equipped parts in Equipped section
    }

    /**
     * Precondition: the part is currently in the user's inventory
     * Postcondition: the part is on the current bot;
     */
    private bool AddPartToBot(PartInfo part)
    {
        InventoryItem itemToRemove = userInventory.First(item => item.GetPart().GetID() == part.GetID());
        if ( itemToRemove.DecreaseCount())
        {
            currentBot.AddPart(part);
            return true; 
        }
        else
        {
            return false;
        }
    }

    /**
     * Precondition: the part is currently on the bot
     * Postcondition: the part is no longer on the bot and is added to the user's inventory
     */
    private bool RemovePartFromBot(PartInfo part)
    {
        currentBot.RemovePart(part);
        InventoryItem userItem = userInventory.First(item => item.GetPart().GetID() == part.GetID());
        if (userItem != null)
        {
            userItem.IncreaseCount();
        }
        //It was the user's only copy of a part and we wi

        ;
        return true;
    }
    
    // Start is called before the first frame update
    //Start Here
    /**
     * 
     */
    void Start()
    {
        HasChanged();

        
    }

    // Update is called once per frame
    void Update()
    {
        HasChanged();
        
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