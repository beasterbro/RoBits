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
    private void Start()
    {


       if (!DataManager.Instance.AuthEstablished) DataManager.Instance.EstablishAuth("DEV testUser@gmail.com");
       StartCoroutine(DataManager.Instance.FetchInitialDataIfNecessary(success =>
       {
           if (!success) return;
            //Acutally needed code
            StartCoroutine(DataManager.Instance.FetchAllParts(delegate(bool obj)
            {
                allParts = DataManager.Instance.AllParts;
                InstantiateStoreButtons();
                RefreshCurrency();
            }));

        }));


        //Gives all buttons desired attributes and functionality


    }


    //Called in editor on change, gathers all of the current buttons
    private void OnValidate()
    {
        ClearButtons();
     //   GatherAllButtons();
    }

    //Shows the buy menu to the user to allow them to purchase a part
    private void ShowBuyMenu(StoreButton storeButton)
    {
        if (validUserLevel(storeButton))
        {
            ShowBuyMenu(storeButton.part.ID);
        }
        else
        {
            ;//The user cannot purchase the part
        }
    }

    private bool validUserLevel(StoreButton storeButton)
    {
        return storeButton.Part.LevelToUnlock <= DataManager.Instance.CurrentUser.Level;
    }

    //Adds parts to the StoreButtons,then adds their images, then removes unavailable parts
    private void InstantiateStoreButtons()
    {
        GatherAllButtons();
        GiveStoreButtonsActions();
        AddPartsToButtons();
        AddImagesToButtons();
        ShowAvailableParts();


    }

    private void GiveStoreButtonsActions()
    {
       for (int i = 0; i < StoreButtons.Count; i++)
               {
                   //On Click offer option to purchase item
                   StoreButtons[i].OnRightClickEvent+= ShowBuyMenu;
                   StoreButtons[i].OnLeftClickEvent += ShowBuyMenu;

                   //For hovering over an item, shows item info
                   StoreButtons[i].OnPointerEnterEvent += ShowPartSpecs;
                   StoreButtons[i].OnPointerExitEvent += HidePartSpecs;
               }
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
            if (storeButton.Part == null)
            {
                storeButton.gameObject.SetActive(false);
            }
            else
            {
                if (!validUserLevel(storeButton))
                {
                    var buttoncolor = storeButton.Image.color;

                   buttoncolor = new Color(buttoncolor.r,buttoncolor.g,buttoncolor.b,0.5f);
                   storeButton.Image.color = buttoncolor;
                }
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

        private void ClearButtons()
        {
            foreach (var button in StoreButtons)
            {
                button.Part = null;
                button.Image = null;
            }
        }

        public void RefreshCurrency()
        {
            StartCoroutine(DataManager.Instance.UpdateCurrentUser(success =>
            {
                if (!success)
                {
                    Debug.Log("No Currency");
                    return;
                }
                UserCurrency.text = DataManager.Instance.CurrentUser.Currency.ToString();
            }));

        }

        public void Buy()
        {
            BuyOptionMenu.Buy();
            RefreshCurrency();
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }
}
