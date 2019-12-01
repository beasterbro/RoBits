using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
//Character
public class InventoryController : MonoBehaviour
{

    //For integration with BE
    private List<BotInfo> userBots;
    private  List<InventoryItem> userInventory;
    [SerializeField] private MonoBehaviour latch;

    public BotInfo currentBot ;
    //Game Objects to attatch generated bot images to
    [SerializeField] private List<GameObject> BotGenerators;

    //Displaying info to user
    [SerializeField] private PartDescHud partDesc;
    [FormerlySerializedAs("sellMenu")] [SerializeField] private SellMenu SellOptionMenu;
    [SerializeField] Text botNameText;
    [SerializeField] private Text Currency;
    [SerializeField] private Text botInfoText;
    [SerializeField] private Text newBotName;

    //Managing user icunput
    [SerializeField]  Inventory inventory;
    [SerializeField] EquipmentPanel equipmentPanel;

    //Managing Dragged and Dropped items
    private ItemSlot draggedItemSlot;
    //Drag and Drop does not work if DraggableItem has RayCast Target Enabled
    [SerializeField] Image draggableItem;


    //Called every time Unity compiles scripts
    private void OnValidate()
    {
        if (partDesc == null)
        {
            partDesc = FindObjectOfType<PartDescHud>();
        }

    }

    //Deletes all of the children of the inputted gameObject
    //This is to prevent duplicate parts being shown to the user when the scene is changed
    void ClearBotImage(GameObject botGenrator)
    {
        BotPreviewGenerator.ClearBotImage(botGenrator);
    }

    //Generates an image for the bot via the BotPreviewGenerator using the given bot info and the object
    //the Image is to be generated onto
    void CreateBotImage(BotInfo botInfo, GameObject botGenerator)
    {
        BotPreviewGenerator.CreateBotImage(botInfo,botGenerator);
    }

    //Delegates the actions to an appropriate method
    private void Awake()
    {

        // Setup Events:
        // Right Click
        Inventory.OnRightClickEvent += Equip;
        EquipmentPanel.OnRightClickEvent += Unequip;
        // Left Click
        Inventory.OnLeftClickEvent += ShowSellMenu;
        // Pointer Enter
        Inventory.OnPointerEnterEvent += ShowPartDesc;
        EquipmentPanel.OnPointerEnterEvent += ShowPartDesc;

        // Pointer Exit
        Inventory.OnPointerExitEvent += HidePartDesc;
        EquipmentPanel.OnPointerExitEvent += HidePartDesc;

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

    void ShowSellMenu(ItemSlot itemSlot)
    {
        SellOptionMenu.transform.position = MousePosition();

        SellOptionMenu.ShowSellMenu(itemSlot.Item.Part);
    }

    public void Sell()
    {
        SellOptionMenu.Sell();
        RefreshCurrency();
        RefreshInventory();
    }
    public void HideSellMenu()
    {
        SellOptionMenu.CancelSellMenu();
    }

    //A method for when ItemSlots are dragged
    private void BeginDrag(ItemSlot itemSlot)
    {
        if (itemSlot.Item != null)
        {
            draggedItemSlot = itemSlot;
            draggableItem.sprite = itemSlot.Item.icon;


            draggableItem.transform.position = MousePosition();

            draggableItem.gameObject.SetActive(true);
        }
    }

    //Calculates the current mouse position
    Vector3 MousePosition()
    {
        var v3 = Input.mousePosition;
        v3.z = 1;
        v3 = Camera.main.ScreenToWorldPoint(v3);
        return v3;
    }

    //Runs through the neccesary procedure when a user stops dragging an item
    private void EndDrag(ItemSlot itemSlot)
    {
        draggedItemSlot = null;
        draggableItem.gameObject.SetActive(false);
    }

    //The procedure to be run when an Item is currently being dragged by the user
    //sets the draggableItem to the current mouse position
    private void Drag(ItemSlot itemSlot)
    {

        draggableItem.transform.position = MousePosition();


    }

    //The procedure to be run when the user drops an item
    //The item shall be put into the slot it is dropped on if the slot can receive the item
    private void Drop(ItemSlot dropItemSlot)
    {
        if (dropItemSlot == null) return;
        //TODO: Fix Dupe with dropping item out and then swapping bots and getting new item
        if (dropItemSlot.CanReceiveItem(draggedItemSlot.Item) && draggedItemSlot.CanReceiveItem(dropItemSlot.Item))
        {
            Item dragItem = draggedItemSlot.Item;
            Item dropItem = dropItemSlot.Item;
            SwapItems(dropItem,dragItem,dropItemSlot);
        }

    }



    //Equips the inputted itemSlot part to the current Bot
    private void Equip(ItemSlot itemSlot)
    {
        Item item = itemSlot.Item;
        if (item != null)
        {
            Equip(item);
        }
    }

    //Unequips the inputted itemSlot part from the current Bot
    private void Unequip(ItemSlot itemSlot)
    {
        Item item = itemSlot.Item;
        if (item != null)
        {
            Unequip(item);
        }
    }

    //Shows the tool tip for a given item slot if the item slot is not empty
    public void ShowPartDesc(ItemSlot itemSlot)
    {
        if (itemSlot.Item != null)
        {
            partDesc.ShowItemInfo(itemSlot.Item);
        }
    }

    //Hides the tool tip from the UI
    public void HidePartDesc(ItemSlot itemSlot)
    {

        partDesc.HideToolTip();

    }









    //Swaps the inputted item with the currently stored draggedItemSlot item
    private void SwapItems(Item dropItem, Item dragItem,ItemSlot dropItemSlot)
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
        int draggedItemAmount = draggedItemSlot.Amount;

        draggedItemSlot.Item = dropItemSlot.Item;
        draggedItemSlot.Amount = draggedItemAmount;

    }

    //Removes inputted item from inventory and adds it to equip panel
    public void Equip(Item item)
    {
        //If you can remove the part
        if (inventory.RemoveItem(item) && item != null)
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
                currentBot.AddPart(item.Part);
                RefreshBotInfo();
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
            currentBot.RemovePart(item.Part);
            RefreshBotInfo();
            CreateAllBotImages();
        }
    }

    //Sets the currently active bot
    public void SetActiveBot(int botValue)
    {

        currentBot = userBots[botValue];
        botNameText.text = currentBot.Name;
        RefreshBotInfo();
        RefreshEquipment();
        //   UpdateInventory();
    }

    private void RefreshBotInfo()
    {
        StringBuilder sb = new StringBuilder();
        foreach (var part in currentBot.Equipment)
        {
            sb.Append(part.Name + ": ");
            sb.Append(part.Description);
            sb.AppendLine();
        }

        botInfoText.text = sb.ToString();
    }

    //Refreshes the displayed Equipment from the current Bot's parts
    public void RefreshEquipment()
    {
        // SetEquipmentMax(currentBot);
        //Retrieves all of the items currently equipped and store them
        List<Item> addToInventory = equipmentPanel.ClearEquippedItems();

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
    public static Item UserItemToItem(InventoryItem inventoryItem)
    {
        Item item = ScriptableObject.CreateInstance<Item>();
        item.InventoryItem = inventoryItem;

        item.icon = PartImageGenrator.GenerateImage(item.Part.ResourceName);
        item.MaximumStacks = 999;


        return item;
    }

    public static Item PartToItem(PartInfo part)
    {
        Item item = ScriptableObject.CreateInstance<Item>();
        item.Part = part;

        item.icon = PartImageGenrator.GenerateImage(item.Part.ResourceName);

        return item;
    }


    private void Update()
    {
        RefreshCurrency();
    }


    //Called First when entering playmode, before the first frame
    void Start()
    {
        DataManager.Instance.Latch(latch);
        if (!DataManager.Instance.AuthEstablished) DataManager.Instance.EstablishAuth("DEV testUser@gmail.com");
        StartCoroutine(DataManager.Instance.FetchInitialDataIfNecessary(success =>
        {
            if (!success) return;

            userInventory = DataManager.Instance.UserInventory;
            userBots = new List<BotInfo>(DataManager.Instance.AllBots);
            SetActiveBot(0);
            CreateAllBotImages();

            RefreshCurrency();
            RefreshInventory();
            RefreshEquipment();
            RefreshBotInfo();
        }));
    }

    //Generates all of the bot images for the current user's bots
    private void CreateAllBotImages()
    {
        BotPreviewGenerator.BotGenerators = BotGenerators;
        BotPreviewGenerator.CreateAllBotImages();
    }

    //Called on start to add all of the items in the user's inventory to the inventory panel
    public void RefreshInventory()
    {
        ClearInventory();
        StartCoroutine(DataManager.Instance.FetchUserInventory(delegate(bool obj)
        {
            foreach (var inventoryItem in userInventory)
            {
                inventory.AddItem((UserItemToItem(inventoryItem).GetCopy()));
            }
        }));

    }

    private void ClearInventory()
    {
        inventory.Clear();
    }

    //Updates the shown currency value to the actual currency value
    private void RefreshCurrency()
    {
        StartCoroutine(DataManager.Instance.FetchInitialDataIfNecessary(success =>
        {
            Currency.text = DataManager.Instance.CurrentUser.Currency.ToString();
        }));
    }

    public void ChangeBotName()
    {
        currentBot.Name = newBotName.text;
        botNameText.text = currentBot.Name;
    }



    //TODO: Must Call This Before Disabling Inventory
    public void UpdateUserBots()
    {
        DataManager.Instance.Latch(latch);
        foreach (var bot in userBots)
        {
            StartCoroutine(DataManager.Instance.UpdateBot(bot, success =>
            {
                if (!success) return;
                Console.WriteLine("success");
                    
                
            }));
        }


    }

    private void OnApplicationQuit()
    {
        UpdateUserBots();
    }

    private void OnDisable()
    {
       // UpdateUserBots();
        StopAllCoroutines();
    }

    private void UpdateUserInformation()
    {
        UpdateUserBots();
        UpdateUserInventory();

    }

    private void UpdateUserInventory()
    {
       //TODO:Finish this last method
    }
}