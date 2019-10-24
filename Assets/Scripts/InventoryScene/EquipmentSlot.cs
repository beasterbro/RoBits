using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSlot : ItemSlot
{
 //  public PartType PartType;

   protected override void OnValidate()
   {
      base.OnValidate();
      gameObject.name =  " Slot";
   }
}
