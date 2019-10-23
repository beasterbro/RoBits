using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

/*
 * Represents an inventory item
 */
public class Item : MonoBehaviour
{
    //The image of the item to display
    [SerializeField] public Image itemImage;
    //the amount of the item the user has
    private InventoryItem _inventoryItem;
    // Start is called before the first frame update

    private void Start()
    {
        itemImage.name = _inventoryItem.GetPart().GetName();
        
    }

    public void SetInventoryItem(InventoryItem item)
    {
        _inventoryItem = item;
    }

    public InventoryItem GetInventoryItem()
    {
        return _inventoryItem;
    }
}
