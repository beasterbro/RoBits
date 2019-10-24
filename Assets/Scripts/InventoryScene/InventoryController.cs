using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
//Character
public class InventoryController : MonoBehaviour 
{
    private static List<BotInfo> userBots;
    public static BotInfo currentBot;
    private static List<InventoryItem> userInventory;
    public List<Image> itemImages;

    // [SerializeField] Transform equipedSlots;
   
    //  [SerializeField] Transform inventorySlots;
    [SerializeField] ItemToolTip itemToolTip;
    [SerializeField] Text inventoryText;
    [SerializeField] Text testText;
    [SerializeField] Text botInfoText;
    
    public BotInfo activeBot;
    private ItemSlot draggedSlot;

    [SerializeField] Image draggableItem;
    [SerializeField]  Inventory inventory;
    [SerializeField] EquipmentPanel equipmentPanel;


    private void OnValidate()
    {
        if (itemToolTip == null)
        {
            itemToolTip = FindObjectOfType<ItemToolTip>();
        }
    }

    private void Awake()
    {

        // Setup Events:
        // Right Click
        Inventory.OnRightClickEvent += Equip;
        EquipmentPanel.OnRightClickEvent += Unequip;
        // Pointer Enter
        Inventory.OnPointerEnterEvent += ShowTooltip;
        EquipmentPanel.OnPointerEnterEvent += ShowTooltip;
        
        // Pointer Exit
        Inventory.OnPointerExitEvent += HideTooltip;
        EquipmentPanel.OnPointerExitEvent += HideTooltip;
       
        // Begin Drag
        Inventory.OnBeingDragEvent += BeginDrag;
        EquipmentPanel.OnBeingDragEvent += BeginDrag;
        // End Drag
        Inventory.OnEndDragEvent += EndDrag;
        EquipmentPanel.OnEndDragEvent += EndDrag;
        // Drag
        Inventory.OnDragEvent += Drag;
        EquipmentPanel.OnDragEvent += Drag;
        // Drop
        Inventory.OnDropEvent += Drop;
        EquipmentPanel.OnDropEvent += Drop;
       
    }

    private void BeginDrag(ItemSlot itemSlot)
    {
        if (itemSlot.Item != null)
        {
            draggedSlot = itemSlot;
            draggableItem.sprite = itemSlot.Item.Icon;
            draggableItem.transform.position = Input.mousePosition;
            draggableItem.enabled = true;
        }
    }

    private void EndDrag(ItemSlot itemSlot)
    {
        draggedSlot = null;
        draggableItem.enabled = false;
    }

    private void Drag(ItemSlot itemSlot)
    {
        draggableItem.transform.position = Input.mousePosition;
    }

    private void Drop(ItemSlot itemSlot)
    {
        Item draggedItem = draggedSlot.Item;
        itemSlot.Item = draggedItem;
    }

    private void Equip(ItemSlot itemSlot)
    {
        Item item = itemSlot.Item;
        if (item != null)
        {
            Equip(item);
        }
    }
    
    private void Unequip(ItemSlot itemSlot)
    {
        Item item = itemSlot.Item;
        if (item != null)
        {
            Unequip(item);
        }
    }
    
    private void ShowTooltip(ItemSlot itemSlot)
    {
        if (itemSlot.Item != null)
        {
            itemToolTip.ShowTooltip(itemSlot.Item);
        }
    }

    private void HideTooltip(ItemSlot itemSlot)
    {
        
        itemToolTip.HideToolTip();
        
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