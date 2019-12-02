using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectOpposingMenu : MonoBehaviour
{
    [SerializeField] private List<GameObject> BotPreviews;

    public TeamInfo[] enemyTeams;
    public UserInfo enemy;
    
    void Start()
    {
        DataManager.Instance.Latch(this);

        if (enemy == null)
        {
            StartCoroutine(DataManager.Instance.SearchUser("trainingUser1", (userExists, userInfo) =>
            {
                if (!userExists)
                {
                    Debug.Log("No Find Trainer");
                }
                else
                {
                    enemy = userInfo;              
                    Debug.Log(userInfo.Email);
                    
                }
                
                StartCoroutine(DataManager.Instance.GetOtherUserTeams(enemy.ID, (success, opponentTeams) =>
                {
                    if (!success) return;

                    enemyTeams = opponentTeams;

                    IEnumerator<GameObject> preview = BotPreviews.GetEnumerator();
                    preview.MoveNext();
                    foreach (var team in enemyTeams)
                    {
                        foreach (var bot in team.Bots)
                        {
                            BotPreviewGenerator.CreateBotImage(bot, preview.Current);
                            preview.MoveNext();
                        }
                    }

                    preview.Dispose();
                }));
                
            }));
            return;
        }
        else
        {
            StartCoroutine(DataManager.Instance.GetOtherUserTeams(enemy.ID, (success, opponentTeams) =>
            {
                if (!success) return;

                enemyTeams = opponentTeams;

                IEnumerator<GameObject> preview = BotPreviews.GetEnumerator();
                preview.MoveNext();
                foreach (var team in enemyTeams)
                {
                    foreach (var bot in team.Bots)
                    {
                        BotPreviewGenerator.CreateBotImage(bot, preview.Current);
                        preview.MoveNext();
                    }
                }

                preview.Dispose();
            }));
        }




    }

    public void SelectOpposingTeam(int team)
    {
        BattleController.opponentTeam = enemyTeams[team];
        SceneManager.LoadScene(Scenes.Simulation);
    }

}