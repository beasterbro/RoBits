using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Image = UnityEngine.Experimental.UIElements.Image;

public class InventoryController : MonoBehaviour 
{
    private static List<BotInfo> userBots;
    public static BotInfo currentBot;
    private static List<InventoryItem> userInventory;
    public List<Image> itemImages;

   // [SerializeField] Transform equipedSlots;
   
 //  [SerializeField] Transform inventorySlots;
    [SerializeField] Text inventoryText;
    [SerializeField] Text testText;
    [SerializeField] Text botInfoText;
 
    public BotInfo activeBot;


    [SerializeField] static Inventory inventory;
    [SerializeField] EquipmentPanel equipmentPanel;


    private void Awake()
    {
        inventory.OnItemRightClickedEvent += Equip;
        equipmentPanel.OnItemRightClickedEvent += Unequip;
    }

    public void Equip(Item item)
    {
        if (inventory.RemoveItem(item))
        {
            //Implementation for specific slots
            if (equipmentPanel.AddItem(item,out var previousItem))
            {
                if (previousItem != null)
                {
                    inventory.AddItem(previousItem);
                }
            }
            else
            {
                inventory.AddItem(item);
            }
        }
    }

    public void Unequip(Item item)
    {
        if (!inventory.IsFull() && equipmentPanel.RemoveItem(item))
        {
            inventory.AddItem(item);
        }
    }

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
       
       
    private static PartInfo part1 = new PartInfo(000, "0", "first part", PartType.Weapon, 1, 1, true, attributes1);
    private static PartInfo part2 = new PartInfo(001, "1", "second part", PartType.Weapon, 2, 2, true, attributes2);

    private static List<PartInfo> allParts = new List<PartInfo>(new PartInfo[]{part1,part2});
    private static List<PartInfo> allParts2 = new List<PartInfo>(new PartInfo[]{part2,part1});
        
    private static PartInfo body = new PartInfo(002, "body", "thrid part", PartType.BodyType, 2, 2, false, bodySpec);

    private static UserInfo user = new UserInfo("testUser","ass@ass.com","tester101",100,200,true,settings);
       
    private static BotInfo bot0 = new BotInfo(0,"bot0",0,allParts,body);
    private static BotInfo bot1 = new BotInfo(1,"bot1",1,allParts,body);
    private static BotInfo bot2 = new BotInfo(2,"bot2",2,allParts2,body);

    private static BotInfo[] botTeam = new[] {bot0, bot1, bot2};
    
    private static InventoryItem item1 = new InventoryItem(part1,1);
    private static InventoryItem item2 = new InventoryItem(part1,100);
    
    
    
    /*
     * calls purchase part from DataManager
     */
    public static bool PurchaseItem(Item item)
    {
        if ( user.GetCurrency() >= item.price && inventory.AddItem(item))
        {
            user.SetCurrency(user.GetCurrency() - item.price);
           InventoryItem possibleItem = userInventory.FirstOrDefault(userItem => userItem.GetPart().GetID() == item.id);
           if (possibleItem == null)
           {
               userInventory.Add(new InventoryItem(item.part,1));
           }
           else
           {
               possibleItem.IncreaseCount();
           }
           return true;
        }
        
       
        return false;
        // return DataManager.instance().PurchasePart(item);
    }

    public static bool SellPart(Item item)
    {
        if (inventory.RemoveItem(item))
        {
            user.SetCurrency((int) (user.GetCurrency() + 0.2 * item.price));
            userInventory.First(invitem => invitem.GetPart().GetID() == item.id).DecreaseCount();
            return true;
        }

        return false;

        //return DataManager.instance().SellPart(item);
    }

    public void SetActiveBot(int botValue)
    {
        //for testing purposes of setting the actively edited bot
        testText.text = "" + botValue;
        // currentBot = DataManager.instance().GetAllBots()[botValue];
        currentBot = botTeam[botValue];
        testText.text = currentBot.GetName();
        botInfoText.text = " Part 1: " + currentBot.GetEquippedParts()[0].GetDescription() + " Part 2: " + currentBot.GetEquippedParts()[1].GetDescription();
        //TODO: Remove currently equipped parts from inventory
        //TODO: Show currently equipped parts in Equipped section
    }

 // Start is called before the first frame update
    //Start Here
    /**
     * 
     */
    void Start()
    {
 
        //userInventory = DataManager.instance().GetUserInventory();
        userInventory = new List<InventoryItem>{item1,item2};
        
    }
    

    // Update is called once per frame
    void Update()
    {
       
        
    }

 }