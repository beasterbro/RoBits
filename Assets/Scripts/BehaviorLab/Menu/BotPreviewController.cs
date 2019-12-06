using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BotPreviewController : MonoBehaviour
{
    private static BotPreviewController Instance;
    
    [SerializeField] private GameObject botPreviewListContainer;
    private List<GameObject> selectableBotPreviews = new List<GameObject>(9);
    [SerializeField] private GameObject currentBotPreview;
    [SerializeField] private Text currentBotName;

    private void Start()
    {
        Instance = this;

        foreach (Transform selectable in botPreviewListContainer.transform)
        {
            if (selectable.transform.childCount > 0)
            {
                selectableBotPreviews.Add(selectable.transform.GetChild(0).gameObject);
            }
        }
    }

    public static void UpdateCurrentPreview(BotInfo botInfo)
    {
        BotPreviewGenerator.CreateBotImage(botInfo, Instance.currentBotPreview);
        Instance.currentBotName.text = botInfo.Name;
    }
    
    //Generates all of the bot images for the current user's bots
    public static void CreateBotPreviews()
    {
        BotPreviewGenerator.BotGenerators = Instance.selectableBotPreviews;
        BotPreviewGenerator.CreateAllBotImages();
    }
}
