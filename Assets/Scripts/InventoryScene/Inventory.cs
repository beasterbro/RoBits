using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Inventory : MonoBehaviour
{//TODO: Implement Specified Item Categories
   
    [SerializeField] List<Item> startingitems;
    [SerializeField] Transform itemsParent;
    [SerializeField] ItemSlot[] itemSlots;
    
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
        for (int i = 0; i < itemSlots.Length; i++)
        {
            itemSlots[i].OnPointerEnterEvent += OnPointerEnterEvent;
            itemSlots[i].OnPointerExitEvent += OnPointerExitEvent;
            itemSlots[i].OnRightClickEvent += OnRightClickEvent;
            itemSlots[i].OnBeingDragEvent += OnBeingDragEvent;
            itemSlots[i].OnDragEvent += OnDragEvent;
            itemSlots[i].OnEndDragEvent += OnEndDragEvent;
            itemSlots[i].OnDropEvent += OnDropEvent;
        }
    }

    private void OnValidate()
    {
        if (itemsParent != null)
        {
            itemSlots = itemsParent.GetComponentsInChildren<ItemSlot>();
        }
        SetStartingItems();
    }

    private void SetStartingItems()
    {
        int i = 0;
        for (; i < startingitems.Count && i < itemSlots.Length; i++)
        {
            itemSlots[i].Item = startingitems[i];
        }

        for (; i < itemSlots.Length; i++)
        {
            itemSlots[i].Item = null;
        }
    }

    public bool AddItem(Item item)
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            //if (itemSlots[i].Item == null && itemSlots[i].CanReceiveItem(item))
            // if (itemSlots[i].PartType == item.type)
            if (itemSlots[i].Item == null)
            {
                itemSlots[i].Item = item;
                return true;
            }
        }

        return false;
    }

    public bool RemoveItem(Item item)
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i].Item == item)
            {
                itemSlots[i].Item = null;
                return true;
            }
        }

        return false;
    }

    public bool IsFull()
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i].Item == null)
            {
               
                return false;
            }
        }

        return true;
    }

}