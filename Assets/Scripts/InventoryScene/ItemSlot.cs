using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour , IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IDragHandler,IBeginDragHandler,IEndDragHandler,IDropHandler
    {
        [SerializeField] Image image;
        [SerializeField] public PartType PartType;
        [SerializeField] private Text amountText;
       
        public event Action<ItemSlot> OnRightClickEvent;
        public event Action<ItemSlot> OnLeftClickEvent;
        public event Action<ItemSlot> OnPointerEnterEvent;
        public event Action<ItemSlot> OnPointerExitEvent;
        public event Action<ItemSlot> OnPointerClickEvent;
        public event Action<ItemSlot> OnBeingDragEvent;
        public event Action<ItemSlot> OnDragEvent;
        public event Action<ItemSlot> OnEndDragEvent;
        public event Action<ItemSlot> OnDropEvent;
        
        private  Color normalColor = Color.white;
        private  Color disableColor = new Color(1,1,1,0);
        
        [SerializeField] private Vector2 originalPosition;

        private int amount;

        public int Amount
        {
            get => amount;
            set
            {
                amount = value;
                amountText.enabled = _item != null && _item.MaximumStacks > 1 && amount > 1;
                if (amountText.enabled)
                {
                    amountText.text = amount.ToString();
                }
            }
        }
        private Item _item;
        public Item Item
        {
            get { return _item; }
            set
            {
                _item = value;
                if (_item == null)
                {
                    image.color= disableColor;
                }
                else
                {
                    image.sprite = _item.icon;
                    image.color = normalColor;
                }
                
                    
                
            }
        }

        //Checks whether this can receive an item
        public virtual bool CanReceiveItem(Item item)
        {
        
            if (item == null)
            {
            return true;
                
            }
            return item != null && item.Type == PartType;
        }
       
       
        //Only runs in editor
        protected virtual void OnValidate()
        {
            if (image == null)
            {
                image =  GetComponent<Image>();
            }

            if (amountText == null)
            {
                amountText = GetComponentInChildren<Text>();
            }

        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData != null && eventData.button == PointerEventData.InputButton.Right)
            {
                if ( OnRightClickEvent != null)
                {
                    OnRightClickEvent(this);
                }
            }
            if (eventData != null && eventData.button == PointerEventData.InputButton.Left)
            {
                if ( OnLeftClickEvent != null)
                {
                    OnLeftClickEvent(this);
                }
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if ( OnPointerEnterEvent != null)
            {
                OnPointerEnterEvent(this);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if ( OnPointerExitEvent != null)
            {
                OnPointerExitEvent(this);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if ( OnDragEvent != null)
            {
                OnDragEvent(this);
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if ( OnBeingDragEvent != null)
            {
                OnBeingDragEvent(this);
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if ( OnEndDragEvent != null)
            {
                OnEndDragEvent(this);
            } 
        }

        public void OnDrop(PointerEventData eventData)
        {
            if ( OnDropEvent != null)
            {
                OnDropEvent(this);
            }
        }

    }