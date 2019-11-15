using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuyMenu : MonoBehaviour
{
    // Start is called before the first frame update
    PartInfo[] AllParts = DataManager.Instance.AllParts;

    private int PartID;
    void Start()
    {
     
    }

    public void ShowBuyMenu(int partID)
    {
        gameObject.SetActive(true);
        PartID = partID;
    }

    public void Buy(int PartID)
    {
        DataManager.Instance.PurchasePart(AllParts.ToList().Find(info => info.ID == PartID));
    }

     public void CancelBuyMenu()
    {
        gameObject.SetActive(false);
    }
}
