using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleBotPreview : MonoBehaviour
{
  [SerializeField] private List<GameObject> BotPreviews;

  private async void Start()
  {
    DataManager.Instance.EstablishAuth("lucaspopp0@gmail.com");
    DataManager.Instance.FetchInitialData();
    
    BotPreviewGenerator.BotGenerators = BotPreviews;
    BotPreviewGenerator.CreateAllBotImages();
  }
}
