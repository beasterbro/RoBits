﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Inventory : MonoBehaviour
{
   
    [SerializeField] List<Item> startingitems;
    [SerializeField] Transform itemsParent;
    [SerializeField] ItemSlot[] itemSlots;

    public ItemSlot[] ItemSlots
    {
        get => itemSlots;
    }
    
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

    //Instantiates the starting items and adds them to the Inventory
    private void SetStartingItems()
    {
        int i = 0;
        for (; i < startingitems.Count && i < itemSlots.Length; i++)
        {
            itemSlots[i].Item = Instantiate(startingitems[i]);
            itemSlots[i].Item.Icon = ItemImageGenrator.GenerateImage(itemSlots[i].Item.part.ResourceName);
        }

        for (; i < itemSlots.Length; i++)
        {
            itemSlots[i].Item = null;
        }
    }

    //Adds an item to the inventory, returns false if the item cannot be added
    public virtual bool AddItem(Item item)
    {
        //for (int i = 0; i < itemSlots.Length; i++)
        foreach (var slot in itemSlots)
        {
           
            // if (itemSlots[i].PartType == item.type)
            //if (itemSlots[i].Item == null)
            if (slot.CanReceiveItem(item) && slot.Item == null)
            {
                slot.Item = item;
                return true;
            }
        }

        return false;
    }

    //Removes an item from the inventory, returns falso if the item cannot be removed
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

    public Item RemoveItem(string itemID)
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            Item item = itemSlots[i].Item;
            if (item != null && item.ID == itemID)
            {
                itemSlots[i].Item = null;
                return item;
            }
        }

        return null;
    }

    //Returns whether or not the inventory is full 
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

    public int ItemCount(string itemID)
    {
        int number = 0;

        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i].Item.ID == itemID)
            {
                number++;
            }
        }

        return number;
    }

}