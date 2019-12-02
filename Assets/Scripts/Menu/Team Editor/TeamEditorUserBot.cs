using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class TeamEditorUserBot : MonoBehaviour , IPointerClickHandler
{

    private BotInfo botInfo;


    public BotInfo BotInfo
    {
        get => botInfo;
        set => botInfo = value;
    }
    public event Action<TeamEditorUserBot> OnLeftClickEvent;
    public event Action<TeamEditorUserBot> OnRightClickEvent;
    
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


}
