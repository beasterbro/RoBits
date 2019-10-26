using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour , IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IDragHandler,IBeginDragHandler,IEndDragHandler,IDropHandler
    {
        [SerializeField] Image image;
      //  [SerializeField] private PartType PartType;
       
        public event Action<ItemSlot> OnRightClickEvent;
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
                    image.sprite = _item.Icon;
                    image.color = normalColor;
                }
                
                    
                
            }
        }

       /* public virtual bool CanReveiveItem(Item item)
        {
            //TODO: Implment
            if (item == null)
            {
            return true;
                
            }
            return item != null && item.type == PartType;
        }*/
       
       
        //Only runs in editor
        protected virtual void OnValidate()
        {
            if (image == null)
            {
                image =  GetComponent<Image>();
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