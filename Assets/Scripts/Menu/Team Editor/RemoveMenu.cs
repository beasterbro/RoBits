using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveMenu : MonoBehaviour
{
    public TeamBot TeamBot;
    public void ShowRemoveMenu(TeamBot teamBot)
    { 
        gameObject.SetActive(true);
        TeamBot = teamBot;
        
    }
    
    public void CancelRemoveMenu()
    {
        TeamBot = null;
        gameObject.SetActive(false);
        
    }
}
