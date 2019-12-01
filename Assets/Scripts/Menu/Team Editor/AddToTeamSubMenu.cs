using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddToTeamSubMenu : MonoBehaviour
{
    public int  Team;
    public int TeamSlot;
   
    public void ShowAddToTeamSubMenu(int  team)
    { 
        gameObject.SetActive(true);
        Team = team;
        
    }
    
    public void CancelAddToTeamSubMenu()
    {
        Team = 0;
        gameObject.SetActive(false);
        
    }
}
