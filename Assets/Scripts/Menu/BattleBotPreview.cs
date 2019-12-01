using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleBotPreview : MonoBehaviour
{
    [SerializeField] private List<GameObject> BotPreviews;

    private TeamInfo[] userTeams;

    private void Start()
    {
        StartCoroutine(DataManager.Instance.UpdateCurrentUser(success =>
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

    private void OnEnable()
    {
        StartCoroutine(DataManager.Instance.UpdateCurrentUser(success =>
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