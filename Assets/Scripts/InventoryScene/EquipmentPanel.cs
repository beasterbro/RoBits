using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentPanel : MonoBehaviour
{
    [SerializeField] Transform equipmentSlotsParent;
    [SerializeField] private EquipmentSlot[] equipmentSlots;

    public event Action<Item> OnItemRightClickedEvent;

    private void Start()
    {
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            equipmentSlots[i].OnRightClickEvent += OnItemRightClickedEvent;
        }
    }
    
    private void OnValidate()
    {
        equipmentSlots = equipmentSlotsParent.GetComponentsInChildren<EquipmentSlot>();
    }

    public bool AddItem(Item item,out  Item previousItem)
    {
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            //TODO: Implementation for Specialized Slots
            if (equipmentSlots[i].PartType == item.type)
            {
                previousItem = equipmentSlots[i].Item;
                equipmentSlots[i].Item = item;
                return true;
            }

        }
        previousItem = null;
        return false;
    }

    public bool RemoveItem(Item item)
    {
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            //TODO: Implementation for Specialized Slots
            if (equipmentSlots[i].Item == item)
            {
                equipmentSlots[i].Item = null;
                return true;
            }

           
        }  
        return false;
    }
}