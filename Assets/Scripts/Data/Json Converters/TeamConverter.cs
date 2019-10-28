using System;
using System.Linq;
using System.Collections.Generic;

namespace JsonData
{

    public class TeamConverter : Converter<TeamInfo>
    {

        public override void SerializeJson(SerializationHelper serializer, TeamInfo obj)
        {
            serializer.WriteKeyValue<int>("tid", obj.GetID());
            serializer.WriteKeyValue<string>("name", obj.GetName());
            serializer.WriteKeyValue<string>("last_maintenance", obj.GetDateLastMaintained().ToString("MM/dd/yyyy"));
            serializer.WriteKeyValue<double>("rank", obj.GetRank());
            serializer.WriteKeyValue<int>("tier", obj.GetTier());
            serializer.SerializeKeyValue<int[]>("bots",
                obj.GetBots().Select<BotInfo, int>(bot => bot.GetID()).ToArray());
            serializer.WriteKeyValue<string>("uid", obj.GetUserID());
        }

        public override TeamInfo DeserializeJson(DeserializationHelper helper)
        {
            string uid = helper.GetValue<string>("uid");
            int id = helper.GetValue<int>("tid");
            string name = helper.GetValue<string>("name", "");
            string lastMaintenance = helper.GetValue<string>("last_maintenance", "");
            double rank = helper.GetValue<double>("rank", 0);
            int tier = helper.GetValue<int>("tier", 0);

            BotInfo[] bots;

            if (uid == DataManager.GetManager().GetCurrentUser().GetID())
            {
                int[] botIds = helper.GetValue<int[]>("bots", new int[0]);
                List<BotInfo> botList = new List<BotInfo>(botIds.Select(DataManager.GetManager().GetBot));
                botList.RemoveAll(bot => bot == null);
                bots = botList.ToArray();
            }
            else
            {
                bots = helper.GetArrayValue("bots", new BotInfo[0]);
            }

            return new TeamInfo(id, name, DateTime.Parse(lastMaintenance), bots, rank, tier, uid);
        }

    }

}