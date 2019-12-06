using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotPreviewGenerator : MonoBehaviour
{
    
    public static List<GameObject> BotGenerators;

    /*
    private void GenerateBotImage(BotInfo botInfo)
    {
        BodyTypeController body;
        BotInfo info = botInfo;
        body = PartController.ControllerForPart(info.BodyType) as BodyTypeController;
        body.PositionWeapons();
    }*/
   public static void CreateBotImage(BotInfo botInfo, GameObject botGenerator)
    {   
        ClearBotImage(botGenerator);
        botGenerator.AddComponent<BotController>().LoadInfoForPreview(botInfo);

        RectTransform rect = botGenerator.GetComponent<RectTransform>();
        Vector2 scale = new Vector3(rect.rect.width, rect.rect.height, 1);
        foreach (Transform child in botGenerator.transform)
        {
            child.localScale *= scale;
        }
    }

   public static void ClearBotImage(GameObject botGenerator)
   {
       foreach (Transform child in botGenerator.transform)
       {
           Destroy(child.gameObject);
       }
       Destroy(botGenerator.GetComponent<BotController>());
   }
   
   public static void CreateAllBotImages()
   {
       List<BotInfo> userBots = new List<BotInfo>(DataManager.Instance.AllBots);
       IEnumerator<BotInfo> BotInfoGenerator = userBots.GetEnumerator();
       // BotInfoGenerator.Reset();
       BotInfoGenerator.MoveNext();
       foreach (var botObject in BotGenerators)
       {    
           CreateBotImage(BotInfoGenerator.Current,botObject);
           BotInfoGenerator.MoveNext();
       }
       BotInfoGenerator.Dispose();
   }
}
