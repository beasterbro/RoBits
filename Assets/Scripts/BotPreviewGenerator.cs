using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotPreviewGenerator : MonoBehaviour
{
    
    public static List<GameObject> BotGenerators;
    static TeamInfo teamInfo = new TeamInfo(1,"tempTeam",new DateTime(1,1,1),DataManager.Instance.AllBots ,1,1,DataManager.Instance.CurrentUser.ID  );


    private void GenerateBotImage(BotInfo botInfo)
    {
        BodyTypeController body;
        BotInfo info = botInfo;
        body = PartController.ControllerForPart(info.BodyType) as BodyTypeController;
        body.PositionWeapons();
    }
   public static void CreateBotImage(BotInfo botInfo, GameObject botGenerator)
    {   
        ClearBotImage(botGenerator);
        botGenerator.AddComponent<BotController>().LoadInfo(botInfo,teamInfo); 
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
