using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sensors;

public class TriggerManager
{
    private readonly static Dictionary<int, TriggerInfo> triggers = new Dictionary<int, TriggerInfo>();
    private readonly static Dictionary<SensorType, List<TriggerInfo>> sensorsToTriggers = new Dictionary<SensorType, List<TriggerInfo>>();

    // TODO: have the database store the trigger information so it's not dependent on the creation on system and have this loaded in from DataManager instead of created as follows
    static TriggerManager()
    {
        string[] names = { "Idle", "OnEnter", "OnExit", "OnWithin" };

        List<SensorType> types = SensorTypeHelper.NonNullTypes;

        foreach (SensorType type in types)
        {
            for (int i = 0; i < names.Length; i++)
            {
                int id = 100 * types.IndexOf(type) + i;
                AddTrigger(id, type, names[i]);
            }
        }
    }

    private static void AddTrigger(int id, SensorType type, string name)
    {
        TriggerInfo trigger = new TriggerInfo(id, type, name);

        triggers.Add(id, trigger);

        if (!sensorsToTriggers.ContainsKey(type))
        {
            sensorsToTriggers.Add(type, new List<TriggerInfo>());
        }
        sensorsToTriggers[type].Add(trigger);
    }

    public static TriggerInfo GetTrigger(int id)
    {
        return triggers[id];
    }

    public static List<TriggerInfo> GetTriggersFor(SensorType sensor)
    {
        return sensorsToTriggers.ContainsKey(sensor) ? sensorsToTriggers[sensor] : new List<TriggerInfo>();
    }

    public static List<TriggerInfo> GetTriggersFor(List<SensorType> sensors)
    {
        List<TriggerInfo> triggers = new List<TriggerInfo>();
        foreach (SensorType sensor in sensors)
        {
            triggers.AddRange(GetTriggersFor(sensor));
        }
        return triggers;
    }
}
