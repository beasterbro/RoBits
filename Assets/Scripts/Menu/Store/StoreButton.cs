using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StoreButton : MonoBehaviour , IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private  Color disableColor = new Color(1,1,1,0);
    private  Color normalColor = Color.white;
    public PartType Type;
    public PartInfo part;
    [SerializeField] private Image image;
    
    public event Action<StoreButton> OnLeftClickEvent;
    public event Action<StoreButton> OnRightClickEvent;
    public event Action<StoreButton> OnPointerEnterEvent;
    public event Action<StoreButton> OnPointerExitEvent;

    public Image Image
    {
        get
        {
            image.color = part==null ? disableColor : normalColor;
            return image;
        }

        set => image = value;
    }

    public PartInfo Part
    {
        get => part;
        set => part = value;

    }
    
    

    private void OnValidate()
    {
        gameObject.name = Type.ToString();
        Image = gameObject.GetComponent<Image>();

    }


    public void OnPointerClick(PointerEventData eventData)
    {
            
        if (eventData != null && eventData.button == PointerEventData.InputButton.Left)
        {
            if ( OnLeftClickEvent != null)
            {
                OnLeftClickEvent(this);
            }
        }
        
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
            OnPointerEnterEvent(this);
        }
    }
}