using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TeamBot : MonoBehaviour , IPointerClickHandler
{
    private BotInfo botInfo;
    private TeamInfo currentTeam;

    public TeamInfo CurrentTeam
    {
        get => currentTeam;
        set => currentTeam = value;
    }

    public BotInfo BotInfo
    {
        get => botInfo;
        set => botInfo = value;
    }
    public event Action<TeamBot> OnLeftClickEvent;
    public event Action<TeamBot> OnRightClickEvent;
    
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
