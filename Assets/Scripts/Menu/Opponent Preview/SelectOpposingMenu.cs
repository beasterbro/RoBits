using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectOpposingMenu : MonoBehaviour
{
    [SerializeField] private List<GameObject> BotPreviews;

    public TeamInfo[] enemyTeams;
    public UserInfo enemy;
    
    void Start()
    {
        StartCoroutine(DataManager.Instance.GetOtherUserTeams(enemy.ID, ( success, opponentTeams ) =>
        {
            if (!success) return;
            
            enemyTeams = opponentTeams;
            
            IEnumerator<GameObject> preview = BotPreviews.GetEnumerator();
            preview.MoveNext();
            foreach (var team in enemyTeams)
            {
                foreach (var bot in team.Bots)
                {
                    BotPreviewGenerator.CreateBotImage(bot,preview.Current);
                    preview.MoveNext();
                }
            }
        
            preview.Dispose();
            
        }));
        
       
    }

    public void SelectOpposingTeam(int team)
    {
        BattleController.opponentTeam = enemyTeams[team];
    }

}
