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
    private  List<InventoryItem> userInventory;
    
    
    public BotInfo currentBot ;
    public List<Image> itemImages;


    //Displaying info to user
    [SerializeField] ItemToolTip itemToolTip;
    [SerializeField] Text testText;
    [SerializeField] Text botInfoText;
    
    //Managing user input
    [SerializeField]  Inventory inventory;
    [SerializeField] EquipmentPanel equipmentPanel;
   
    //Managing Dragged and Dropped items
    private ItemSlot draggedItemSlot;
    //Drag and Drop does not work if DraggableItem has RayCast Target Enabled
    [SerializeField] Image draggableItem;
 
 

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
            draggedItemSlot = itemSlot;
            draggableItem.sprite = itemSlot.Item.Icon;
            draggableItem.transform.position = Input.mousePosition;
            draggableItem.gameObject.SetActive(true);
        }
    }

    private void EndDrag(ItemSlot itemSlot)
    {
        draggedItemSlot = null;
        draggableItem.gameObject.SetActive(false);
    }

    private void Drag(ItemSlot itemSlot)
    {
     
        draggableItem.transform.position = Input.mousePosition;
            
        
    }

    private void Drop(ItemSlot dropItemSlot)
    {
        if (draggedItemSlot == null) return;
        //TODO: Work on can receive method so that empty slots can be considered
        if (dropItemSlot.CanReceiveItem(draggedItemSlot.Item) && draggedItemSlot.CanReceiveItem(dropItemSlot.Item))
        {
            SwapItems(dropItemSlot);
        }
       
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
       
       
    
    private PartInfo tankGun= new PartInfo(112, "Tank Gun", "Shoot Shells", PartType.Weapon, 100, 2, true, attributes1);
    private PartInfo gun = new PartInfo(113, "Base Gun", "Shoot Bullets", PartType.Weapon, 1, 0, true, attributes2);
    private PartInfo baseBody = new PartInfo(222, "Base Body", "Body of Bot", PartType.BodyType, 1, 1, true, attributes1);
    private PartInfo armor = new PartInfo(333, "Reflective Armor", "Reflects Bullets", PartType.Armor, 1, 1, true, attributes1);
    private PartInfo wheels = new PartInfo(444, "Wheels", "High Speed, Low Load", PartType.Transport, 1, 1, true, attributes1);
    private PartInfo treads = new PartInfo(555, "Tank Treads", "Low Speed High Load", PartType.Transport, 1, 1, true, attributes1);
    
    
    private PartInfo body = new PartInfo(222, "body", "Main Body part", PartType.BodyType, 2, 2, false, bodySpec);

    private UserInfo user = new UserInfo("testUser","ass@ass.com","tester101",100,200,true,settings);
     

    private  List<BotInfo> botTeam = new List<BotInfo>();
    

    
    private void SwapItems(ItemSlot dropItemSlot)
    {
        Item dragEquipItem = draggedItemSlot.Item;
        Item dropEquipItem = dropItemSlot.Item;
        if (draggedItemSlot is EquipmentSlot && dropItemSlot is EquipmentSlot)
        {
            Item tempItem = draggedItemSlot.Item;
            draggedItemSlot.Item = dropItemSlot.Item;
            dropItemSlot.Item = tempItem;
            return;
        }
        if (draggedItemSlot is EquipmentSlot)
        {
            if (dragEquipItem != null) Equip(dragEquipItem);
            if (dropEquipItem != null) Unequip(dropEquipItem);
        }
        if (dropItemSlot is EquipmentSlot)
        {
            if (dragEquipItem != null) Unequip(dragEquipItem);
            if (dropEquipItem != null) Equip(dropEquipItem);
        }
        Item draggedItem = draggedItemSlot.Item;
        draggedItemSlot.Item = dropItemSlot.Item;
        dropItemSlot.Item = draggedItem;
        
    }
    
    //Removes inputted item from inventory and adds it to equip panel 
    public void Equip(Item item)
    {
        //If you can remove the part
        if (inventory.RemoveItem(item))
        {
            //If you can add it to the equipped
            if (equipmentPanel.AddItem(item,out var previousItem))
            {
               
                //if equipped is full and it swaps items
                if (previousItem != null)
                {
                    inventory.AddItem(previousItem);
                    Unequip(previousItem);
                }
                currentBot.AddPart(item.part);
            }
            else
            {
                inventory.AddItem(item);
            }
        }
    }
   
    //Removes inputted item from equip panel and adds it to the inventory
    public void Unequip(Item item)
    {
        if (equipmentPanel.RemoveItem(item))
        {
            inventory.AddItem(item);
            currentBot.RemovePart(item.part);
        }
    }

    public void SetActiveBot(int botValue)
    {
        //TODO: Current Implementation bug, hard wired bot parts reset upon switch
        //for testing purposes of setting the actively edited bot
        // currentBot = DataManager.instance().GetAllBots()[botValue];
        currentBot = userBots[botValue];
        UpdateEquipment();
    }

    public void UpdateEquipment()
    {
        
        //Retrieves all of the items currently equipped and store them
        List<Item> addToInventory = equipmentPanel.ClearEquipped();
       
        Item previousItem;
        foreach (PartInfo part in currentBot.GetEquippedParts())
        {
        //Creating new item to add to equipment panel

            Item item = partToItem(part);
            equipmentPanel.AddItem(item,out previousItem);
            
            inventory.AddItem(previousItem);
        }
    }

    //Converts inputted part into an item to be put in the inventory
    Item partToItem(PartInfo part)
    {
        Item item = new Item();
        item.part = part;
        item.partID = part.GetID();
        item.type = part.GetPartType();
        item.price = part.GetPrice();
        item.description = part.GetDescription();
        item.Icon = part.GetSprite();
        item.attributes = part.GetAttributes();
        item.ItemName = part.GetName();
        item.levelToUnlock = part.GetLevelToUnlock();
        item.isActor = part.IsActor();
        item.Icon = this.GetComponentInParent<ItemImageGenrator>().generateImage(part.GetID());

        return item;
    }


    void Start()
    {
        //Temp vars for testing
        var item1 = new InventoryItem(treads,1);
        var item2 = new InventoryItem(baseBody,2);
        var item3 = new InventoryItem(gun,1);
        var  item4 = new InventoryItem(tankGun,1);
        
        
        var allParts = new List<PartInfo>(new PartInfo[]{tankGun,treads,armor});
        var allParts2 = new List<PartInfo>(new PartInfo[]{gun,wheels,baseBody});
        var allParts3 = new List<PartInfo>(new PartInfo[]{tankGun,gun});
        
        
        var bot0 = new BotInfo(0,"bot0",0,allParts,baseBody);
        var bot1 = new BotInfo(1,"bot1",1,allParts2,baseBody);
        var bot2 = new BotInfo(2,"bot2",2,allParts3,baseBody);
        
        
        botTeam.Add(bot0);
        botTeam.Add(bot1);
        botTeam.Add(bot2);
        userBots = botTeam;
        //userInventory = DataManager.instance().GetUserInventory();
        userInventory = new List<InventoryItem>{item1,item2,item3,item4};
        currentBot = userBots[0];
        UpdateInventory();
        UpdateEquipment();
        
    }

    private void UpdateInventory()
    {
        foreach (var inventoryItem in userInventory)
        {
            inventory.AddItem(partToItem(inventoryItem.GetPart()));
        }
    }

}