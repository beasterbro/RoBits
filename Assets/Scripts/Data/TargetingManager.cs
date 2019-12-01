using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TargetingManager
{
    // Returns nth target or last target if less than n total or first target if n is less than 1 or null if there were no targets
    public static BotController NthTarget(int n, string targetingPriority, BotController myself)
    {
        List<BotController> targets = Targets(targetingPriority, myself);
        int working_n = Mathf.Clamp(n, 1, targets.Count); // Clamp n between "1st" and "last"
        return targets.Count > 0 ? targets[working_n - 1] : null;
    }

    public static List<BotController> Targets(string targetingPriority, BotController myself)
    {
        return TargetFunctions.PriorityManager.ApplyTargetPriority(targetingPriority, myself, myself.TargetableBots());
    }

    public static List<string> TargetingPriorities(string sensor)
    {
        return TargetFunctions.PriorityManager.PrioritiesFrom(sensor);
    }
}

namespace TargetFunctions
{
    internal class PriorityManager
    {
        private static Dictionary<string, List<Target>> sensorTargets = new Dictionary<string, List<Target>>();
        internal static Dictionary<Target, PriorityFunction> targetFunctions = new Dictionary<Target, PriorityFunction>();

        static PriorityManager()
        {
            // Add sensor specific target priorities here TODO: could use more
            {
                AddTargets("ProximitySensor", Target.CLOSEST, Target.FARTHEST);
                AddTargets("VisionSensor", Target.CLOSEST, Target.FARTHEST);
            }

            PriorityFunctionHelper.AttachPriorityFunctions();
        }

        private static void AddTargets(string sensor, params Target[] targets)
        {
            if (!sensorTargets.ContainsKey(sensor)) sensorTargets.Add(sensor, new List<Target>());
            sensorTargets[sensor].AddRange(targets);
        }

        internal static List<string> PrioritiesFrom(string sensor)
        {
            List<string> priorities = new List<string>();
            foreach (Target priority in sensorTargets[sensor])
            {
                priorities.Add(TargetPriorityHelper.ToString(priority));
            }
            return priorities;
        }

        internal static List<BotController> ApplyTargetPriority(string targetPriority, BotController myself, List<BotController> them)
        {
            return targetFunctions[TargetPriorityHelper.Parse(targetPriority)](myself, them);
        }
    }
}
