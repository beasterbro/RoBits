using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSlot : ItemSlot
{
    


    /*public override bool CanReveiveItem(Item item)
    {
        return true;
    }*/
    
    protected override void OnValidate()
    {
        base.OnValidate();
        gameObject.name =  " Slot";
    }
}