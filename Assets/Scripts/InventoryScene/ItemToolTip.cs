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
 public void ShowTooltip(Item item)
 {
  ItemNameText.text = item.ItemName;
  ItemSlotText.text = item.type.ToString();

  //Add more Stats here
  sb.Length = 0;
  AddStat(item.price.ToString(), "Price");
  AddStat(item.levelToUnlock.ToString(), "Unlock At");
  AddStat(item.id.ToString(), "ID");
  AddStat(item.description,"desc");

  ItemStatText.text = sb.ToString();
  
  gameObject.SetActive(true);
 }

 public void HideToolTip()
 {
  gameObject.SetActive(false);
 }

 private void AddStat(String info, string statName)
 {
  String value = info.ToString();
 // if (value != 0)
  //{
   if (sb.Length > 0)
   {
    sb.AppendLine();
   }

   double doubleVal;

   if (double.TryParse(value, out doubleVal))
   {
    sb.Append("+");
   }

   sb.Append(value);
   sb.Append(" ");
   sb.Append(statName);
 // }
 }
}
