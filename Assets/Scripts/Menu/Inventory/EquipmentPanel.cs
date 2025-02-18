﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentPanel : MonoBehaviour
{
    [SerializeField] Transform equipmentSlotsParent;
    [SerializeField] private EquipmentSlot[] equipmentSlots;

    public static event Action<ItemSlot> OnRightClickEvent;
    public static event Action<ItemSlot> OnPointerEnterEvent;
    public static event Action<ItemSlot> OnPointerExitEvent;
    public static event Action<ItemSlot> OnBeingDragEvent;
    public static event Action<ItemSlot> OnDragEvent;
    public static event Action<ItemSlot> OnEndDragEvent;
    public static event Action<ItemSlot> OnDropEvent;

    //Assigns each equipmentSlot all of the desired actions
    private void Start()
    {
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            equipmentSlots[i].OnPointerEnterEvent += OnPointerEnterEvent;
            equipmentSlots[i].OnPointerExitEvent += OnPointerExitEvent;
            equipmentSlots[i].OnRightClickEvent += OnRightClickEvent;
            equipmentSlots[i].OnBeingDragEvent += OnBeingDragEvent;
            equipmentSlots[i].OnDragEvent += OnDragEvent;
            equipmentSlots[i].OnEndDragEvent += OnEndDragEvent;
            equipmentSlots[i].OnDropEvent += OnDropEvent;
        }
    }
    
    private void OnValidate()
    {
        equipmentSlots = equipmentSlotsParent.GetComponentsInChildren<EquipmentSlot>();
    }

    //Adds an item to the equipment panel
    public bool AddItem(Item item,out  Item previousItem)
    {
        
        foreach (var slot in equipmentSlots)
        {

            if (slot.CanReceiveItem(item) && slot.Item == null )
            {
               // if ( slot.Item != null)
                {
                    previousItem = slot.Item;
                    slot.Item = item;
                    return true; 
                }
                /*else
                {
                    previousItem = null;
                    slot.Item = item;
                    return true; 
                }*/
                
            }
        }
        previousItem = null;
        return false;
  
    }

    //removes an item from the equipment panel
    public bool RemoveItem(Item item)
    {
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            //Can change the implementation to add specialized slots
            if (equipmentSlots[i].Item == item)
            {
                equipmentSlots[i].Item = null;
                return true;
            }

           
        }  
        return false;
    }

    //Clears the currently equipped and return them as a list
    public List<Item> ClearEquippedItems()
    {
        List<Item> equippedItems = new List<Item>();
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            equippedItems.Add(equipmentSlots[i].Item);
            equipmentSlots[i].Item = null;
        }

        return equippedItems;
    }
}