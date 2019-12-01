using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectTeamMenuController : MonoBehaviour
{

    public int battleType;

    public void SelectTeam(int teamIndex)
    {
        if (DataManager.Instance.UserTeams.Length > teamIndex)
        {
            Debug.Log("Attempted to load nonexistent team");
        }
        else
        {
            BattleController.playerTeam = DataManager.Instance.UserTeams[teamIndex];
            
            if (battleType == 2) GoToOpponentSearch();
            else GoToBattle();
        }
    }

    private void GoToBattle()
    {
        SceneManager.LoadScene(Scenes.Simulation);
    }

    private void GoToOpponentSearch()
    {
        SceneManager.LoadScene(Scenes.OpponentSelection);
    }

}