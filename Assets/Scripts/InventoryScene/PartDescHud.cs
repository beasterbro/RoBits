using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class PartDescHud : MonoBehaviour
{
     [SerializeField] Text PartNameText;
     [SerializeField] Text PartTypeText;
     [SerializeField] Text PartStatText;

    private StringBuilder sb = new StringBuilder();
    //Displays the tooltip with all of the specified stats added
    public void ShowItemInfo(Item item)
    {
        PartNameText.text = item.ItemName;
        PartTypeText.text = item.Type.ToString();

        //Add more Stats here
        sb.Length = 0;
        AddStat(item.Price.ToString(), "Price:");
        AddStat(item.LevelToUnlock.ToString(), "Unlock At:");
        AddStat(item.PartID.ToString(), "ID:");
        AddStat(item.Description,"Desc:");

        PartStatText.text = sb.ToString();
  
        gameObject.SetActive(true);
    }

    public void ShowPartInfo(PartInfo part)
    {
        PartNameText.text = part.Name;
        PartTypeText.text = part.PartType.ToString();

        //Add more Stats here
        sb.Length = 0;
        AddStat(part.Price.ToString(), "Price:");
        AddStat(part.LevelToUnlock.ToString(), "Unlock At:");
        AddStat(part.ID.ToString(), "ID:");
        AddStat(part.Description,"Desc:");

        PartStatText.text = sb.ToString();
  
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
    
   
   

   

    }
}