using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectTeamMenuController : MonoBehaviour
{

    public int battleType;

    public void SelectTeam(int teamIndex)
    {
        if (DataManager.Instance.UserTeams.Length <= teamIndex)
        {
            Debug.Log("Attempted to load nonexistent team");
        }
        else
        {
            BattleController.playerTeam = DataManager.Instance.UserTeams[teamIndex];
            if (battleType == 4)
            {
                GoToTraining();
            }
            else if (battleType == 2)
            {
                GoToOpponentSearch();
            }
            else GoToBattle();
              
            
                
            
            
            
        }
    }

    private void GoToBattle()
    {
        SceneManager.LoadScene(7);
    }

    private void GoToTraining()
    {
        SceneManager.LoadScene(6);
    }

    private void GoToOpponentSearch()
    {
        SceneManager.LoadScene(Scenes.OpponentSelection);
    }

}