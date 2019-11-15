using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

//Generates the Image for an Item from a specified partID
public class ItemImageGenrator : MonoBehaviour
{
     static List<Sprite> PartImages = Resources.LoadAll<Sprite>("InventoryItems/").ToList();

    public static Sprite GenerateImage( String partName)
    {
        foreach (var sprite in PartImages)
        {
            if (sprite.name == partName.ToString())
            {
                return sprite;
            }
        }

        return null;
    }
}
