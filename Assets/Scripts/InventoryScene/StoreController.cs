using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

//A script to manage the store interface
public class StoreController : MonoBehaviour
{
    
    [SerializeField] private ItemToolTip itemToolTip;
    [SerializeField] private BuyMenu BuyOptionMenu;
    [SerializeField] private GameObject StoreButtonParent;
    private List<StoreButton> StoreButtons = new List<StoreButton>();

    private PartInfo[] allParts;
    
    public PartInfo[] AllParts => allParts;
    
    
    public static event Action<StoreButton> OnLeftClickEvent;
    public event Action<StoreButton> OnPointerEnterEvent;
    public event Action<StoreButton> OnPointerExitEvent;
    //Shows the tool tip for the part that was clicked on in the store
    public void ShowTooltip(PartInfo partInfo)
    {
        if (partInfo != null)
        {
            itemToolTip.ShowPartInfo(partInfo);
        }
    }

    //Shows the buy menu for the part that was clicked
    public void ShowBuyMenu(int partID)
    {
        BuyOptionMenu.ShowBuyMenu(partID);
    }

    //Called when the Cancel Button is pressed and hides the buy menu and the part info
    public void CancelBuyMenu()
    {
        BuyOptionMenu.CancelBuyMenu();
        HideTooltip();
    }

    //Shorts the parts by type and displays them in the store
    

    //Hides the tool tip (part information)
    public void HideTooltip()
    {
        itemToolTip.gameObject.SetActive(false);
        itemToolTip.HideToolTip();
        
    }

    //Displays the the part information for the inputted partID
    public void ShowPartSpecs(StoreButton storeButton)
    {
        ShowTooltip(storeButton.part);
        
    }

    public void HidePartSpecs(StoreButton storeButton)
    {
        itemToolTip.gameObject.SetActive(false);
        HideTooltip();
    }

    //Instantiates they parts and adds them to the store based on type
    private async void Start()
    {
   
        
        DataManager.Instance.EstablishAuth("DEV testUser@gmail.com");
        await DataManager.Instance.FetchAllParts();
        allParts = DataManager.Instance.AllParts;
        //  GatherAllButtons();
        
        for (int i = 0; i < StoreButtons.Count; i++)
        {
            //On Click offer option to purchase item
            StoreButtons[i].OnPointerClickEvent += ShowBuyMenu;
            StoreButtons[i].OnRightClickEvent+= ShowBuyMenu;
            StoreButtons[i].OnLeftClickEvent += ShowBuyMenu;
            
            //For hovering over an item, shows item info
            StoreButtons[i].OnPointerEnterEvent += ShowPartSpecs;
            StoreButtons[i].OnPointerExitEvent += HidePartSpecs;
        }
        InstantiateStoreButtons();
    }

    private void OnValidate()
    {
        GatherAllButtons();
    }

    private void ShowBuyMenu(StoreButton storeButton)
    {
        BuyOptionMenu.gameObject.SetActive(true);
        ShowBuyMenu(storeButton.part.ID);
    }

    private void Update()
    {
        
        //     GatherAllButtons();
        //     InstantiateStoreButtons();
    }

    private void InstantiateStoreButtons()
    {
        AddPartsToButtons();
        foreach (var storeButton in StoreButtons)
        {
            storeButton.Image.sprite =ItemImageGenrator.GenerateImage(storeButton.part.ResourceName);
        }
    }

    private void AddPartsToButtons()
    {
        foreach (var part in allParts)
        {
            AddPart(part);
        }
    }

    private void AddPart(PartInfo part)
    {
        foreach (var button in StoreButtons)
        {
            if (button.part == null )//&& button.Type == part.PartType
            {
                button.part = part;
                return;
            }
        }
    }

    private void GatherAllButtons()
    {
       
        foreach (var storeButton in StoreButtonParent.GetComponentsInChildren<StoreButton>())
        {
            StoreButtons.Add(storeButton);
        }
        
    }

}