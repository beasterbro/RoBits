using NUnit.Framework.Constraints;

namespace Tests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using NUnit.Framework;
    using UnityEngine;
    using UnityEngine.TestTools;
    [TestFixture]
    public class Inventory_Test
    {
        private static Dictionary<String,double> attributes1 = new Dictionary<string, double>
        {
            {"DMG",23.0},
            {"DIST",12}
        };
       
        private static Dictionary<String,double> attributes2 = new Dictionary<string, double>
        {
            {"DMG",23.0},
            {"DIST",12}
        };
       
        private static Dictionary<String,string> settings = new Dictionary<string, string>
        {
            {"DARK","yes"},
            {"LOUD","no"}
        };
       
        private static Dictionary<String,double> bodySpec = new Dictionary<string, double>
        {
            {"THICC",11},
            {"SANIC",101}
        };
       
       
        private static PartInfo part1 = new PartInfo(0, "0", "first part", PartType.Weapon, 1, 1, attributes1);
        private static PartInfo part2 = new PartInfo(1, "1", "second part", PartType.Weapon, 2, 2, attributes2);

        private static List<PartInfo> allParts = new List<PartInfo>(new PartInfo[]{part1,part2});
        
        private static PartInfo body = new PartInfo(2, "body", "thrid part", PartType.BodyType, 2, 2, bodySpec);

        private UserInfo _userInfo = new UserInfo("testUser","ass@ass.com","tester101",100,200,true,settings);
       
        private static BotInfo bot0 = new BotInfo(0,"bot0",0,allParts,body);
        private static BotInfo bot1 = new BotInfo(1,"bot1",1,allParts,body);
        private static BotInfo bot2 = new BotInfo(2,"bot2",2,allParts,body);

        private static BotInfo[] botTeam = new[] {bot0, bot1, bot2};

        [Test]
        public void AddItemTestNominal()
        {
            GameObject gameObject = new GameObject();
            gameObject.AddComponent<Inventory>();
            Inventory inventory = gameObject.GetComponent<Inventory>();
            Item item = ScriptableObject.CreateInstance<Item>();
            inventory.AddItem(item);
            Assert.That(inventory.AddItem(item));
        }

    }
}