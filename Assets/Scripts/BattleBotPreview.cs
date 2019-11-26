using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleBotPreview : MonoBehaviour
{
  [SerializeField] private List<GameObject> BotPreviews;

  private void Start()
  {
    BotPreviewGenerator.BotGenerators = BotPreviews;
    BotPreviewGenerator.CreateAllBotImages();
  }
}
