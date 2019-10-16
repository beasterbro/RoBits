using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

/*
 * Represents a part while the user is in the inventory
 */
public class Item : MonoBehaviour
{
    
    [SerializeField] public Image partImage;
    private InventoryItem inventoryItem;
    public PartInfo partInfo ;
    // Start is called before the first frame update

    private void Start()
    {
        partInfo = inventoryItem.GetPart();
        partImage.name = partInfo.GetName();
    }
}
