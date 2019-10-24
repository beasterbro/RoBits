using System;
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
    public event Action<ItemSlot> OnPointerClickEvent;
    public static event Action<ItemSlot> OnBeingDragEvent;
    public static event Action<ItemSlot> OnDragEvent;
    public static event Action<ItemSlot> OnEndDragEvent;
    public static event Action<ItemSlot> OnDropEvent;

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

    public bool AddItem(Item item,out  Item previousItem)
    {
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            if (equipmentSlots[i].Item == null)
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