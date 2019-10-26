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
    
    //For integration with BE
    private List<BotInfo> userBots;
    public BotInfo currentBot ;
    private  List<InventoryItem> userInventory;
    public List<Image> itemImages;


    
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

    private void Drop(ItemSlot dropItemSlot)
    {
        Item draggedItem = draggedSlot.Item;
        draggedSlot.Item = dropItemSlot.Item;
        dropItemSlot.Item = draggedItem;
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


    //Removes inputted item from inventory and adds it to equip panel

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
       
       
    
    private PartInfo part1 = new PartInfo(112, "112", "WeaponPart1", PartType.Weapon, 1, 1, true, attributes1);
    private PartInfo part2 = new PartInfo(113, "113", "WeaponPart2", PartType.Weapon, 2, 2, true, attributes2);
    
    private PartInfo body = new PartInfo(444, "body", "thrid part", PartType.BodyType, 2, 2, false, bodySpec);

    private UserInfo user = new UserInfo("testUser","ass@ass.com","tester101",100,200,true,settings);
     

    private  List<BotInfo> botTeam = new List<BotInfo>();
    

    
    
    
    public void Equip(Item item)
    {
        //If you can remove the part
        if (inventory.RemoveItem(item))
        {
            //If you can add it to the equipped
            if (equipmentPanel.AddItem(item,out var previousItem))
            {
                currentBot.AddPart(item.part);
                //if equipped is full and it swaps items
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
   
    //removes inputted item from equip panel and adds it to the inventory
    public void Unequip(Item item)
    {
        if (!inventory.IsFull() && equipmentPanel.RemoveItem(item))
        {
            currentBot.RemovePart(item.part);
            inventory.AddItem(item);
        }
    }

    public void SetActiveBot(int botValue)
    {
        //TODO: Current Implementation bug, hard wired bot parts reset upon switch
        //for testing purposes of setting the actively edited bot
        testText.text = "" + botValue;
        // currentBot = DataManager.instance().GetAllBots()[botValue];
        currentBot = userBots[botValue];
        testText.text = currentBot.GetName();
        botInfoText.text = " Part 1: " + currentBot.GetEquippedParts()[0].GetDescription() + " Part 2: " + currentBot.GetEquippedParts()[1].GetDescription();
        //TODO: Remove currently equipped parts from inventory
        UpdateEquipment();
    }

    public void UpdateEquipment()
    {
        
        //retrieves all of the items currently equipped and store them
        //TODO: fix duping that happens here (Contains or remove part from bot)
        List<Item> addToInventory = equipmentPanel.ClearEquipped();
        /*  for (int i = 0; i < addToInventory.Count; i++)
          {
              if (!inventory.Contains(addToInventory[i]))
              {
                  inventory.AddItem(addToInventory[i]);
              }
              
          }*/
        
        Item previousItem;
        equipmentPanel.ClearEquipped();
        foreach (PartInfo part in currentBot.GetEquippedParts())
        {
            //Creating new item to add to equipment panel
            Item item = new Item();
            item.part = part;
            item.id = part.GetID();
            item.type = part.GetPartType();
            item.price = part.GetPrice();
            item.description = part.GetDescription();
            item.Icon = part.GetSprite();
            item.attributes = part.GetAttributes();
            item.ItemName = part.GetName();
            item.levelToUnlock = part.GetLevelToUnlock();
            item.isActor = part.IsActor();
            
            equipmentPanel.AddItem(item,out previousItem);
            
            inventory.AddItem(previousItem);
        }
    }


    void Start()
    {
        //Temp for testing
        var item1 = new InventoryItem(part1,1);
        var item2 = new InventoryItem(part2,100);
        
        
        var allParts = new List<PartInfo>(new PartInfo[]{part1,part2});
        var allParts2 = new List<PartInfo>(new PartInfo[]{part2,part1});
        
        
        var bot0 = new BotInfo(0,"bot0",0,allParts,body);
        var bot1 = new BotInfo(1,"bot1",1,allParts,body);
        var bot2 = new BotInfo(2,"bot2",2,allParts2,body);
        
        
        botTeam.Add(bot0);
        botTeam.Add(bot1);
        botTeam.Add(bot2);
        userBots = botTeam;
        //userInventory = DataManager.instance().GetUserInventory();
        userInventory = new List<InventoryItem>{item1,item2};
        currentBot = userBots[0];
        UpdateEquipment();
        
    }
    

    // Update is called once per frame
    void Update()
    {
       
        
    }

}