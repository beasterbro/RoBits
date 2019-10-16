using System;
using System.Linq;
using System.Collections.Generic;

public class TeamConverter : Converter<TeamInfo>
{

    public override void SerializeJson(SerializationHelper serializer, TeamInfo obj)
    {
        serializer.WriteKeyValue<int>("tid", obj.GetID());
        serializer.WriteKeyValue<string>("name", obj.GetName());
        serializer.WriteKeyValue<string>("last_maintenance", obj.GetDateLastMaintained().ToString("MM/dd/yyyy"));
        serializer.WriteKeyValue<double>("rank", obj.GetRank());
        serializer.WriteKeyValue<int>("tier", obj.GetTier());
        serializer.SerializeKeyValue<int[]>("bots", obj.GetBots().Select<BotInfo, int>(bot => bot.GetID()).ToArray());
    }

    public override TeamInfo DeserializeJson(DeserializationHelper helper)
    {
        int id = helper.GetValue<int>("tid");
        string name = helper.GetValue<string>("name", "");
        string lastMaintenance = helper.GetValue<string>("last_maintenance", "");
        double rank = helper.GetValue<double>("rank", 0);
        int tier = helper.GetValue<int>("tier", 0);
        int[] botIds = helper.GetValue<int[]>("bots", new int[0]);

        List<BotInfo> bots = new List<BotInfo>();

        foreach (int botId in botIds)
        {
            foreach (BotInfo bot in DataManager.GetManager().GetAllBots())
            {
                if (bot.GetID() == botId)
                {
                    bots.Add(bot);
                }
            }
        }

        return new TeamInfo(id, name, DateTime.Parse(lastMaintenance), bots.ToArray(), rank, tier);
    }

}