﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSlot : ItemSlot
{
    

    //TODO: Number of slots change depending on body type
    //Overrides the method in ItemSlot to return true since an Equipment slot can always receive an item
    public override bool CanReceiveItem(Item item)
    {
        return true;
    }
    
    protected override void OnValidate()
    {
        base.OnValidate();
        gameObject.name =  "Slot";
    }
}