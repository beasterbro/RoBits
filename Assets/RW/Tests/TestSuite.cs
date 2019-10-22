using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class TestSuite
    {
        /*private static Dictionary<string, double> attributes = null;
        private static Sprite sprite;
        private static PartInfo bodyType = new PartInfo(1,"body","The body",PartType.BodyType,100,1,false,attributes,sprite);
        private static PartInfo weapon = new PartInfo(1,"weapon","The weapon",PartType.Weapon,100,1,false,attributes,sprite);
        private static PartInfo sensor = new PartInfo(1,"sensor","The senson",PartType.Sensor,100,1,false,attributes,sprite);
        private static List<PartInfo> equipment = new List<PartInfo>(new PartInfo[] {weapon,sensor});
        */
        //BotInfo _botInfo = new BotInfo(1,"test",0,equipment,bodyType);
        // A Test behaves as an ordinary method
        [Test]
        public void TestingBotInfoAssigned()
        {
            // Use the Assert class to test conditions
            //Assert.That(_botInfo != null);
        }

        [Test]
        public void TestingBotInfoCorrectInfo()
        {
           
        }

       
        [UnityTest]
        public IEnumerator TestSuiteWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}
