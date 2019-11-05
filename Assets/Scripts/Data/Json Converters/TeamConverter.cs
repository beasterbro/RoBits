using System;
using System.Linq;
using System.Collections.Generic;

namespace JsonData
{

    public class TeamConverter : Converter<TeamInfo>
    {

        protected override void SerializeJson(SerializationHelper serializer, TeamInfo obj)
        {
            serializer.WriteKeyValue("tid", obj.ID);
            serializer.WriteKeyValue("name", obj.Name);
            serializer.WriteKeyValue("last_maintenance", obj.DateLastMaintained.ToString("MM/dd/yyyy"));
            serializer.WriteKeyValue("rank", obj.Rank);
            serializer.WriteKeyValue("tier", obj.Tier);
            serializer.SerializeKeyValue("bots", obj.Bots.Select(bot => bot.ID).ToArray());
            serializer.WriteKeyValue("uid", obj.UserID);
        }

        protected override TeamInfo DeserializeJson(DeserializationHelper helper)
        {
            var uid = helper.GetValue<string>("uid");
            var id = helper.GetValue<int>("tid");
            var name = helper.GetValue("name", "");
            var lastMaintenance = helper.GetValue("last_maintenance", "");
            var rank = helper.GetValue("rank", 0f);
            var tier = helper.GetValue("tier", 0);

            BotInfo[] bots;

            if (uid == DataManager.Instance.CurrentUser.ID)
            {
                var botIds = helper.GetValue("bots", new int[0]);
                var botList = new List<BotInfo>(botIds.Select(DataManager.Instance.GetBot));
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