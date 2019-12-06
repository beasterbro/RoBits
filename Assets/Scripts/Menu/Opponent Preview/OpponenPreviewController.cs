using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponenPreviewController : MonoBehaviour
{
 [SerializeField] private List<GameObject> BotPreviews;

 private void Start()
 {
  IEnumerator<GameObject> BotPreviewsEnum = BotPreviews.GetEnumerator();
  BotPreviewsEnum.MoveNext();
  
  foreach (var botInfo in BattleController.opponentTeam.Bots)
  {
   BotPreviewGenerator.CreateBotImage(botInfo,BotPreviewsEnum.Current);
   BotPreviewsEnum.MoveNext();
  }
  
  BotPreviewsEnum.Dispose();
 }
}
