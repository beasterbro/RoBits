using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddToTeamMenu : MonoBehaviour
{

    public BotInfo BotInfo;
   
    public void ShowAddToTeamMenu(BotInfo botInfo)
    { 
        gameObject.SetActive(true);
        BotInfo = botInfo;
        
    }
    
    public void CancelAddToTeamMenu()
    {
        BotInfo = null;
        gameObject.SetActive(false);
        
    }
}
