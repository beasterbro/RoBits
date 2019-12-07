using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//A scripts to manage the buy menu
public class BuyMenu : MonoBehaviour
{

    //Stores the part ID of the part that activated the buy menu
    private int PartID;
    [SerializeField] private StoreController storeController;
    [SerializeField] private Inventory Inventory;
    void Start()
    {

    }

    //Shows the menu to purchase the part and stores the ID of the part that it was clicked for
    public void ShowBuyMenu(int partID)
    {
        gameObject.SetActive(true);
        PartID = partID;
    }

    //Buys the part via a backend call
    public void Buy()
    {
        PartInfo part = DataManager.Instance.GetPart(PartID);
        if (DataManager.Instance.CurrentUser.Currency >= part.Price)
        {
        DataManager.Instance.CurrentUser.Currency -= part.Price;
        storeController.RefreshCurrency();
        Inventory.AddItem(InventoryController.PartToItem(part));

        }
        gameObject.SetActive(false);
     
      
    }

    //Hides the buy menu when the cancel button is clicked
    public void CancelBuyMenu()
    {
        gameObject.SetActive(false);
        PartID = 0;
    }
}