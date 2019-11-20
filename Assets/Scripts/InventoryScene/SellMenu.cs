using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellMenu : MonoBehaviour
{
    private PartInfo PartInfo;
    public void ShowSellMenu(PartInfo partInfo)
    {
        gameObject.SetActive(true);
        PartInfo = partInfo;
    }

    //Buys the part via a backend call
    public void Sell()
    {
        gameObject.SetActive(false);
        DataManager.Instance.SellPart(PartInfo);

    }

    //Hides the buy menu when the cancel button is clicked
    public void CancelSellMenu()
    {
        gameObject.SetActive(false);
        PartInfo = null;
    }
}
