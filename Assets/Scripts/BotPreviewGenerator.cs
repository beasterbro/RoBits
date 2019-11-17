using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotPreviewGenerator : MonoBehaviour
{
    
    public static List<GameObject> BotGenerators;
    static TeamInfo teamInfo = new TeamInfo(1,"tempTeam",new DateTime(1,1,1),DataManager.Instance.AllBots ,1,1,DataManager.Instance.CurrentUser.ID  );

   public static void CreateBotImage(BotInfo botInfo, GameObject botGenerator)
    {   
        ClearBotImage(botGenerator);
        botGenerator.GetComponent<BotController>().LoadInfo(botInfo,teamInfo); 
    }

   public static void ClearBotImage(GameObject botGenrator)
   {
       foreach (Transform child in botGenrator.transform)
       {
           GameObject.Destroy(child.gameObject);
       }
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
