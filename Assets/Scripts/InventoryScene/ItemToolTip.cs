using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
  AddStat(item.price, "Price");
  AddStat(item.levelToUnlock, "Unlock At");

  ItemStatText.text = sb.ToString();
  
  gameObject.SetActive(true);
 }

 public void HideToolTip()
 {
  gameObject.SetActive(false);
 }

 private void AddStat(double value, string statName)
 {
  if (value != 0)
  {
   if (sb.Length > 0)
   {
    sb.AppendLine();
   }

   if (value > 0)
   {
    sb.Append("+");
   }

   sb.Append(value);
   sb.Append(" ");
   sb.Append(statName);
  }
 }
}
