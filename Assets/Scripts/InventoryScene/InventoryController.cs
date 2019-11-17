using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
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
    //Generates Bot Images
    [SerializeField] private List<GameObject> BotGenerators;

    //Displaying info to user
    [SerializeField] public ItemToolTip itemToolTip;
    [SerializeField] Text botInfoText;
    [SerializeField] private Text Currency;
    
    //Managing user input
    [SerializeField]  Inventory inventory;
    [SerializeField] EquipmentPanel equipmentPanel;
   
    //Managing Dragged and Dropped items
    private ItemSlot draggedItemSlot;
    //Drag and Drop does not work if DraggableItem has RayCast Target Enabled
    [SerializeField] Image draggableItem;
 
 

    private async void OnValidate()
    {
        if (itemToolTip == null)
        {
            itemToolTip = FindObjectOfType<ItemToolTip>();
        }

    }

    void ClearBotImage(GameObject botGenrator)
    {
       BotPreviewGenerator.ClearBotImage(botGenrator);
        }
    void CreateBotImage(BotInfo botInfo, GameObject botGenerator)
    {   
       BotPreviewGenerator.CreateBotImage(botInfo,botGenerator);
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
            
           
            draggableItem.transform.position = MousePosition();
            
            draggableItem.gameObject.SetActive(true);
        }
    }

    Vector3 MousePosition()
    {
        var v3 = Input.mousePosition;
        v3.z = 1;
        v3 = Camera.main.ScreenToWorldPoint(v3);
        return v3;
    }

    private void EndDrag(ItemSlot itemSlot)
    {
        draggedItemSlot = null;
        draggableItem.gameObject.SetActive(false);
    }

    private void Drag(ItemSlot itemSlot)
    {
      
        draggableItem.transform.position = MousePosition();
            
        
    }

    private void Drop(ItemSlot dropItemSlot)
    {
        if (dropItemSlot == null) return;
        //TODO: Fix Dupe with dropping item out and then swapping bots and getting new item
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
    
    public void ShowTooltip(ItemSlot itemSlot)
    {
        if (itemSlot.Item != null)
        {
            itemToolTip.ShowTooltip(itemSlot.Item);
        }
    }

    public void HideTooltip(ItemSlot itemSlot)
    {
        
        itemToolTip.HideToolTip();
        
    }






    

    
    //Swaps the inputted item with the currently stored draggedItemSlot item
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
        //Behavior here could stand being tweaked a bit more
        if (draggedItemSlot is EquipmentSlot)
        {
            Unequip(dragEquipItem);
            Equip(dropEquipItem);
           
        }
        if (dropItemSlot is EquipmentSlot)
        {
            Equip(dragEquipItem);
            Unequip(dropEquipItem);
            
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
                CreateAllBotImages();
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
            CreateAllBotImages();
        }
    }

    //Sets the currently active bot 
    public void SetActiveBot(int botValue)
    {
        //TODO: Current Implementation bug, hard wired bot parts reset upon switch
        //for testing purposes of setting the actively edited bot
        // currentBot = DataManager.Instance().GetAllBots()[botValue];
        currentBot = userBots[botValue];
        botInfoText.text = currentBot.Name;
        UpdateEquipment();
    }

    //Refreshes the displayed Equipment from the current Bot's parts
    public void UpdateEquipment()
    {
       // SetEquipmentMax(currentBot);
        //Retrieves all of the items currently equipped and store them
        List<Item> addToInventory = equipmentPanel.ClearEquipped();
       
        Item previousItem;
        foreach (PartInfo part in currentBot.Equipment)
        {
            //Creating new item to add to equipment panel

            Item item = PartToItem(part);
            equipmentPanel.AddItem(item,out previousItem);
            
            inventory.AddItem(previousItem);
        }
    }

    //Converts inputted part into an item to be put in the inventory
    public Item PartToItem(PartInfo part)
    {
        Item item = ScriptableObject.CreateInstance<Item>();
        item.part = part;
        item.partID = part.ID;
        item.type = part.PartType;
        item.price = part.Price;
        item.description = part.Description;
        item.attributes = part.Attributes;
        item.ItemName = part.Name;
        item.levelToUnlock = part.LevelToUnlock;
        item.Icon = ItemImageGenrator.GenerateImage(part.ResourceName);

        return item;
    }


    async void Start()
    {
        

        DataManager.Instance.EstablishAuth("lucaspopp0@gmail.com");
        await DataManager.Instance.FetchInitialData();
        userInventory = DataManager.Instance.UserInventory;

        SetActiveBot(0);
        CreateAllBotImages();
        
        UpdateInventory();
        UpdateEquipment();
        UpdateCurrency();
        
    }

    private void CreateAllBotImages()
    {
        BotPreviewGenerator.BotGenerators = BotGenerators;
        BotPreviewGenerator.CreateAllBotImages();
    }

    //Called on start to add all of the items in the user's inventory to the inventory panel
    private void UpdateInventory()
    {
        foreach (var inventoryItem in userInventory)
        {
            inventory.AddItem(PartToItem(inventoryItem.Part));
        }
    }

    private void UpdateCurrency()
    {
        Currency.text = DataManager.Instance.CurrentUser.Currency.ToString();
    }
    


}