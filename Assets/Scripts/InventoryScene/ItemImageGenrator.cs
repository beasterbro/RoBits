﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Generates the Image for an Item from a specified partID
public class ItemImageGenrator : MonoBehaviour
{
    [SerializeField] List<Sprite> partImages;

    public Sprite generateImage(int partID)
    {
        foreach (var sprite in partImages)
        {
            if (sprite.name == partID.ToString())
            {
                return sprite;
            }
        }

        return null;
    }
}
