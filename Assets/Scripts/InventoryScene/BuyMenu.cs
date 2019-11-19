using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuyMenu : MonoBehaviour
{
    private int PartID;
    
    public void ShowBuyMenu(int partID)
    {
        gameObject.SetActive(true);
        PartID = partID;
    }

    public void Buy()
    {
        gameObject.SetActive(false);
        DataManager.Instance.PurchasePart(DataManager.Instance.GetPart(PartID));
       
    }

     public void CancelBuyMenu()
    {
        gameObject.SetActive(false);
    }
}
