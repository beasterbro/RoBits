using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class ItemToolTip : MonoBehaviour
{
 [SerializeField] Text ItemNameText;
 [SerializeField] Text ItemSlotText;
 [SerializeField] Text ItemStatText;

 private StringBuilder sb = new StringBuilder();
 //Displays the tooltip with all of the specified stats added
 public void ShowTooltip(Item item)
 {
  ItemNameText.text = item.ItemName;
  ItemSlotText.text = item.type.ToString();

  //Add more Stats here
  sb.Length = 0;
  AddStat(item.price.ToString(), "Price:");
  AddStat(item.levelToUnlock.ToString(), "Unlock At:");
  AddStat(item.partID.ToString(), "ID:");
  AddStat(item.description,"Desc:");

  ItemStatText.text = sb.ToString();
  
  gameObject.SetActive(true);
 }

 public void ShowPartInfo(PartInfo part)
 {
  ItemNameText.text = part.Name;
  ItemSlotText.text = part.PartType.ToString();

  //Add more Stats here
  sb.Length = 0;
  AddStat(part.Price.ToString(), "Price:");
  AddStat(part.LevelToUnlock.ToString(), "Unlock At:");
  AddStat(part.ID.ToString(), "ID:");
  AddStat(part.Description,"Desc:");

  ItemStatText.text = sb.ToString();
  
  gameObject.SetActive(true);
 }

 public void HideToolTip()
 {
  gameObject.SetActive(false);
 }

 //Adds a stat to the toolTip for it to display
 private void AddStat(String info, string statName)
 {
  String value = info.ToString();

   if (sb.Length > 0)
   {
    sb.AppendLine();
   }

    sb.Append(statName);
    sb.Append(" ");
    sb.Append(value);
    
   
   

   
 // }
 }
}
