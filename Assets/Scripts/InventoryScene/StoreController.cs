using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Serialization;
using UnityEngine.UI;

//A script to manage the store interface
public class StoreController : MonoBehaviour
{
    
    [SerializeField] private PartDescHud partDesc;
    [SerializeField] private BuyMenu BuyOptionMenu;
    [SerializeField] private GameObject StoreButtonParent;
    [SerializeField] private Text UserCurrency;
    private List<StoreButton> StoreButtons = new List<StoreButton>();

    private PartInfo[] allParts;
    
    public PartInfo[] AllParts => allParts;
    
    //Shows the tool tip for the part that was clicked on in the store
    public void ShowPartDesc(PartInfo partInfo)
    {
        if (partInfo != null)
        {
            partDesc.ShowPartInfo(partInfo);
        }
    }

    //Shows the buy menu for the part that was clicked
    public void ShowBuyMenu(int partID)
    {
        BuyOptionMenu.transform.position = MousePosition();
        BuyOptionMenu.ShowBuyMenu(partID);
    }

    Vector3 MousePosition()
    {
        var v3 = Input.mousePosition;
        v3.z = 1;
        v3 = Camera.main.ScreenToWorldPoint(v3);
        return v3;
    }
    
    //Called when the Cancel Button is pressed and hides the buy menu and the part info
    public void CancelBuyMenu()
    {
        BuyOptionMenu.CancelBuyMenu();
        HidePartDesc();
    }

    //Shorts the parts by type and displays them in the store
    

    //Hides the tool tip (part information)
    public void HidePartDesc()
    {
        partDesc.gameObject.SetActive(false);
        partDesc.HideToolTip();
        
    }

    //Displays the the part information for the inputted partID
    public void ShowPartSpecs(StoreButton storeButton)
    {
        ShowPartDesc(storeButton.part);
        
    }

    //Hides the specs for the previous part
    public void HidePartSpecs(StoreButton storeButton)
    {
        partDesc.gameObject.SetActive(false);
        HidePartDesc();
    }

    //Instantiates they parts and adds them to the store based on type
    private async void Start()
    {
   
        //Needed for testing
        DataManager.Instance.EstablishAuth("DEV testUser@gmail.com");
        
        //Acutally needed code
        await DataManager.Instance.FetchAllParts();
        allParts = DataManager.Instance.AllParts;
        //  GatherAllButtons();
        
        //Gives all buttons desired actions on given events
        for (int i = 0; i < StoreButtons.Count; i++)
        {
            //On Click offer option to purchase item
            StoreButtons[i].OnRightClickEvent+= ShowBuyMenu;
            StoreButtons[i].OnLeftClickEvent += ShowBuyMenu;
            
            //For hovering over an item, shows item info
            StoreButtons[i].OnPointerEnterEvent += ShowPartSpecs;
            StoreButtons[i].OnPointerExitEvent += HidePartSpecs;
        }
         InstantiateStoreButtons();
        

    }

    

    //Called in editor on change, gathers all of the current buttons
    private void OnValidate()
    {
        GatherAllButtons();
    }

    //Shows the buy menu to the user to allow them to purchase a part
    private void ShowBuyMenu(StoreButton storeButton)
    {
        //  BuyOptionMenu.gameObject.SetActive(true);
        ShowBuyMenu(storeButton.part.ID);
    }

    //Used for testing
    private void Update()
    {
       //InstantiateStoreButtons(); 
        //     GatherAllButtons();
        //     InstantiateStoreButtons();
    }

    //Adds parts to the StoreButtons and then adds their images
    private void InstantiateStoreButtons()
    {
        AddPartsToButtons();
        AddImagesToButtons();
        ShowAvailableParts();
        
    }

    private void AddImagesToButtons()
    {
        foreach (var storeButton in StoreButtons)
        {
            if (storeButton.Part != null)
            {
                storeButton.Image.sprite = PartImageGenrator.GenerateImage(storeButton.Part.ResourceName);
            }
            else
            {
                storeButton.gameObject.SetActive(false);
            }
           
        }
    }
    
    

    //Adds all parts in the store controller to a valid store button
    private void AddPartsToButtons()
    {
        foreach (var part in AllParts)
        {
            AddPart(part);
        }
    }

    //Finds a valid store button to add the part to
    private void AddPart(PartInfo part)
    {
        foreach (var button in StoreButtons)
        {
            if (button.Part == null && button.Type == part.PartType )
            {
                button.Part = part;
                return;
            }
        }
    }
    
    private void ShowAvailableParts()
    {
        foreach (var storeButton in StoreButtons)
        {
            if (storeButton.Part.LevelToUnlock > DataManager.Instance.CurrentUser.Level)
            {
                storeButton.gameObject.SetActive(false);
            }
            else
            {
                //the user can see the part and it is available for purchase
            }
        }
    }

    //Collects all of the store buttons from their parent
    private void GatherAllButtons()
    {
       
        foreach (var storeButton in StoreButtonParent.GetComponentsInChildren<StoreButton>())
        {
            StoreButtons.Add(storeButton);
        }
        
    }

    private void UpdateCurrency()
    {
        UserCurrency.text = DataManager.Instance.CurrentUser.Currency.ToString();
    }

}