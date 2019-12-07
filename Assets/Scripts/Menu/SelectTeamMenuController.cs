using UnityEngine;
using UnityEngine.SceneManagement;
using Random = System.Random;

public class SelectTeamMenuController : MonoBehaviour
{

    private static Random random = new Random();
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

            if (battleType == 0 || battleType == 1)
            {
                StartCoroutine(DataManager.Instance.FetchRandomOpponent((success, opponent) =>
                {
                    if (!success) return;
                    StartCoroutine(DataManager.Instance.GetOtherUserTeams(opponent.ID, (success2, enemyTeams) =>
                    {
                        if (!success2) return;
                        BattleController.opponentTeam = enemyTeams[random.Next(enemyTeams.Length)];
                        GoToBattle();
                    }));
                }));
            }
            else if (battleType == 2)
            {
                GoToOpponentSearch();
            }
            else if (battleType == 4)
            {
                GoToTraining();
            }
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