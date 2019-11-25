using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
        public async  void PartDeserializeTest()
        {
            PartInfo[] allParts = new PartInfo[100];
            var response = await api.GetAsync("/api/parts");

            if (response.IsSuccessStatusCode)
            {
                allParts = JsonUtils.DeserializeArray<PartInfo>(await response.Content.ReadAsStringAsync());
            }
            
            Assert.NotNull(allParts[0]);
        }
       
        [Test]
        public async void UserInventoryFetchTest()
        {
            var  inventory = new List<InventoryItem>();
            var response = await api.GetAsync("/api/inventory");

            if (response.IsSuccessStatusCode)
            {
                inventory = new List<InventoryItem>(
                    JsonUtils.DeserializeArray<InventoryItem>(await response.Content.ReadAsStringAsync()));
            }
            
            Assert.NotNull(inventory[0]);
        }
        
        [Test]
        public async void UserTeamTest()
        {
            var allBots = new List<BotInfo>();
            var  userTeams = new TeamInfo[100];
            var botsResponse = await api.GetAsync("/api/bots");
            var teamsResponse = await api.GetAsync("/api/teams");

            if (botsResponse.IsSuccessStatusCode && teamsResponse.IsSuccessStatusCode)
            {
                allBots = JsonUtils.DeserializeArray<BotInfo>(await botsResponse.Content.ReadAsStringAsync()).ToList();
                userTeams = JsonUtils.DeserializeArray<TeamInfo>(await teamsResponse.Content.ReadAsStringAsync());
                
            }
            
          
            Assert.NotNull(allBots[0]);
            Assert.NotNull(userTeams[0]);
        }

        
        [Test]
        public async void FetchUserTest()
        {
            var user = currentUser;
            var uid = "lmp122";
            var response = await api.GetAsync("/api/user/" + uid);

            if (response.IsSuccessStatusCode)
            {
                user =  JsonUtils.DeserializeObject<UserInfo>(await response.Content.ReadAsStringAsync());
            }
            
            Assert.AreNotEqual(currentUser,user);
        }
        
    }
}