using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

public class BattleBotPreview : MonoBehaviour
{
    [SerializeField] private List<GameObject> BotPreviews;
    public TeamInfo[] userTeams;

    private void Start() => UpdateTeamsAndPreviews();
    private void OnEnable() => UpdateTeamsAndPreviews();

    private void UpdateTeamsAndPreviews()
    {
        DataManager.Instance.Latch(this);
        StartCoroutine(DataManager.Instance.FetchInitialData(success =>
        {
            if (!success) return;
            userTeams = DataManager.Instance.UserTeams;
            
            IEnumerator<GameObject> BotPreviewEnum = BotPreviews.GetEnumerator();
            BotPreviewEnum.MoveNext();
            foreach (var team in userTeams)
            {
                foreach (var bot in team.Bots)
                {
                    BotPreviewGenerator.CreateBotImage(bot,BotPreviewEnum.Current);
                    BotPreviewEnum.MoveNext();
                }
            }
            BotPreviewEnum.Dispose();
        }));
    }
}