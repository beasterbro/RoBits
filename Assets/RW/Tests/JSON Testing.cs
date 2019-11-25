using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Extensions;
using JsonData;
using NUnit.Framework;
using UnityEngine.Networking;

using Debug = UnityEngine.Debug;

namespace Tests
{
    
    public class JSON_Testing
    {
        private static string baseUrl = "http://robits.us-east-2.elasticbeanstalk.com/api";
        private string bearerToken = "DEV testUser@gmail.com";
        private UserInfo currentUser;
        private HttpClient api = new HttpClient();

        [SetUp]
        public void start()
        {
            if (api.BaseAddress == null)
            {
                api.BaseAddress = new Uri("http://robits.us-east-2.elasticbeanstalk.com");
                api.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                api.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
            }
           
        }
        private UnityWebRequest BasicGet(string endpt) => WrapRequest(UnityWebRequest.Get(baseUrl + endpt));
        
        private static void SimpleCallback(UnityWebRequest request, Action action, Action<bool> callback = null)
        {
            if (request.EncounteredError())
            {
                Debug.LogError(request.GetError());
                callback?.Invoke(false);
            }
            else
            {
                try
                {
                    action?.Invoke();
                    callback?.Invoke(true);
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                    callback?.Invoke(false);
                }
            }
        }
        

       
        private UnityWebRequest WrapRequest(UnityWebRequest request)
        {
            request.SetRequestHeader("Authorization", "Bearer " + bearerToken);
            request.SetRequestHeader("Accept", "application/json");
            return request;
        }




        [Test]
        public async  void Part_Deserialize_Test_Nominal()
        {
            PartInfo[] allParts = new PartInfo[100];
            var response = await api.GetAsync("/api/parts");

            if (response.IsSuccessStatusCode)
            {
                allParts = JsonUtils.DeserializeArray<PartInfo>(await response.Content.ReadAsStringAsync());
            }
            
            Assert.NotNull(allParts[0].Name );
        }
        
        [Test]
        public async  void Part_Deserialize_Test_Wrong_Call()
        {
            PartInfo[] allParts = new PartInfo[100];
            var response = await api.GetAsync("/api/par");

            if (response.IsSuccessStatusCode)
            {
                allParts = JsonUtils.DeserializeArray<PartInfo>(await response.Content.ReadAsStringAsync());
            }
            
            Assert.Null(allParts[0]);
        }

   
        public async Task<List<InventoryItem>> Inventory_Deserialize_Async_Nominal()
        {
            var  inventory = new List<InventoryItem>();
            var response = await api.GetAsync("/api/inventory");

            if (response.IsSuccessStatusCode)
            {
                inventory = new List<InventoryItem>(
                    JsonUtils.DeserializeArray<InventoryItem>(await response.Content.ReadAsStringAsync()));
            }
            
            return inventory;
        }
        
        [Test]
        public void Inventory_Deserialize_Test_Nominal()
        {
            Assert.NotNull(Inventory_Deserialize_Async_Nominal().Result);
        }
        
        
        public async Task<List<InventoryItem>> Inventory_Deserialize_Async_Wrong_Call()
        {
            var  inventory = new List<InventoryItem>();
            var response = await api.GetAsync("/api/inv");

            if (response.IsSuccessStatusCode)
            {
                inventory = new List<InventoryItem>(
                    JsonUtils.DeserializeArray<InventoryItem>(await response.Content.ReadAsStringAsync()));
            }
            else
            {
                inventory = null;
            }

            return inventory;
        }
        
        [Test]
        public void Inventory_Deserialize_Test_Wrong_Call()
        {
            try
            {
                Assert.Null(Inventory_Deserialize_Async_Wrong_Call());
                Assert.Fail();
            }
            catch (System.AggregateException e)
            {
                Assert.Pass();
            }
           
        }
        
        
        public async Task<List<BotInfo>> Bot_Deserialize_Async_Nominal()
        {
            var allBots = new List<BotInfo>();
            var botsResponse = await api.GetAsync("/api/bots");
           
            if (botsResponse.IsSuccessStatusCode )
            {
                allBots = JsonUtils.DeserializeArray<BotInfo>(await botsResponse.Content.ReadAsStringAsync()).ToList();
              
            }


            return allBots;
        }
        
        [Test]
        public void Bot_Deserialize_Test_Nominal()
        {
            Assert.NotNull(Bot_Deserialize_Async_Nominal());
        }
        
        public async Task<TeamInfo[]> Team_Deserialize_Async_Nominal()
        {
          
            var  userTeams = new TeamInfo[100];
            var teamsResponse = await api.GetAsync("/api/teams");

            if (teamsResponse.IsSuccessStatusCode)
            {
              
                userTeams = JsonUtils.DeserializeArray<TeamInfo>(await teamsResponse.Content.ReadAsStringAsync());
                
            }
            return userTeams;
        }
        
        [Test]
        public void Team_Deserialize_Test_Nominal()
        {
            Assert.NotNull(Team_Deserialize_Async_Nominal().Result);
        }
        
        public async Task<TeamInfo[]> Team_Deserialize_Async_Wrong_Call()
        {
          
            var  userTeams = new TeamInfo[100];
            var teamsResponse = await api.GetAsync("GARBLED");

            if (teamsResponse.IsSuccessStatusCode)
            {
              
                userTeams = JsonUtils.DeserializeArray<TeamInfo>(await teamsResponse.Content.ReadAsStringAsync());
                
            }
            return userTeams;
        }
        
        [Test]
        public void Team_Deserialize_Test_Wrong_Call()
        {
            try
            {
                Assert.Null(Team_Deserialize_Async_Wrong_Call().Result[0]);
                //Assert.Fail();
            }
            catch (System.AggregateException e)
            {
                Assert.Pass();
            }
           
        }

        public async Task<String> Bot_Serialize_Async_Nominal()
        {
          
            PartInfo part1 = new PartInfo(0, "0", "first part", PartType.Weapon, 1, 1, new Dictionary<string, float>());
            PartInfo part2 = new PartInfo(1, "1", "second part", PartType.Weapon, 2, 2, new Dictionary<string, float>());
            List<PartInfo> allParts = new List<PartInfo>(new PartInfo[]{part1,part2});
            PartInfo body = new PartInfo(2, "body", "thrid part", PartType.BodyType, 2, 2,new Dictionary<string, float>());
            BotInfo bot = new BotInfo(0,"bot0",0,allParts,body, new BehaviorInfo[0]);

            var updateBody = JsonUtils.SerializeObject(bot);
            return updateBody;
        }
        
        [Test]
        public void Bot_Serialize_Test_Nominal()
        {
            Assert.NotNull(Bot_Serialize_Async_Nominal().Result);
        }
        


        
        [Test]
        public void User_Deserialize_Test_Nominal()
        {
            Assert.NotNull(User_Deserialize_Async_Nominal().Result);
        }
        
        public async Task<UserInfo> User_Deserialize_Async_Nominal()
        {
            var user = currentUser;
            var uid = "lmp122";
            var response = await api.GetAsync("/api/user/" + uid);

            if (response.IsSuccessStatusCode)
            {
                user =  JsonUtils.DeserializeObject<UserInfo>(await response.Content.ReadAsStringAsync());
            }
            
            return user;
        }
        
        public async Task<UserInfo> User_Deserialize_Async_Wrong_Call()
        {
            var user = currentUser;
            var uid = "lmp122";
            var response = await api.GetAsync("/api/ur/" + uid);

            if (response.IsSuccessStatusCode)
            {
                user =  JsonUtils.DeserializeObject<UserInfo>(await response.Content.ReadAsStringAsync());
            }
            else
            {
                user = null;
            }

            return user;
        }
        
        [Test]
        public void User_Deserialize_Test_Wrong_Call()
        {
            try
            {
                Assert.That(User_Deserialize_Async_Wrong_Call().Result == null);
               // Assert.Fail();
            }
            catch (System.AggregateException e)
            {
                Assert.Pass();
            }
           
        }
        
       
        public async Task<UserInfo> User_Deserialize_Async_No_User()
        {
            var user = currentUser;
            var uid = "DNE";
            var response = await api.GetAsync("/api/user/" + uid);

            if (response.IsSuccessStatusCode)
            {
                user =  JsonUtils.DeserializeObject<UserInfo>(await response.Content.ReadAsStringAsync());
            }
            else
            {
                user = null;
            }

            return user;
        }
        
        [Test]
        public void User_Deserialize_Test_No_User()
        {
            try
            {
                Assert.That(User_Deserialize_Async_No_User().Result == null);
              //  Assert.Fail();
            }
            catch (System.AggregateException e)
            {
                Assert.Pass();
            }
           
        }
        
    }
}