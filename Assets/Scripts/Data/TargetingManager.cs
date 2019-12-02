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
        return TargetFunctions.TargetFunctionManager.ApplyTargetPriority(targetingPriority, myself, myself.TargetableBots());
    }

    public static bool TargetSatisfiesCondition(string targetingConditional, BotController myself, BotController target)
    {
        return TargetFunctions.TargetFunctionManager.ApplyTargetConditional(targetingConditional, myself, target);
    }

    public static List<string> TargetingPriorities(string sensor)
    {
        return TargetFunctions.TargetFunctionManager.PrioritiesFrom(sensor);
    }

    public static List<string> TargetingConditionals()
    {
        return TargetFunctions.TargetFunctionManager.ConditionalsFrom(BehaviorLabController.CurrentMatchingEquipmentAsResources(PartType.Sensor, true));
    }
}

namespace TargetFunctions
{
    internal class TargetFunctionManager
    {
        private static Dictionary<string, List<Target>> sensorPriorities = new Dictionary<string, List<Target>>();
        internal static Dictionary<Target, PriorityFunction> priorityFunctions = new Dictionary<Target, PriorityFunction>();

        private static Dictionary<string, List<Target>> sensorConditionals = new Dictionary<string, List<Target>>();
        internal static Dictionary<Target, ConditionalFunction> conditionalFunctions = new Dictionary<Target, ConditionalFunction>();

        static TargetFunctionManager()
        {
            // Add sensor specific target priorities here TODO: could use more
            {
                AddPriorities("ProximitySensor", Target.CLOSEST, Target.FARTHEST);
                AddPriorities("VisionSensor", Target.CLOSEST, Target.FARTHEST, Target.LOWEST_HEALTH, Target.HIGHEST_HEALTH);
            }

            PriorityFunctionHelper.AttachFunctions();

            // Add sensor specific target conditionals here TODO: could use more
            {
                AddConditionals("ProximitySensor", Target.IS_TOO_CLOSE, Target.IN_SHORT_RANGE, Target.IN_MEDIUM_RANGE, Target.IN_LONG_RANGE);
                AddConditionals("VisionSensor", Target.SHIELDS_ARE_DOWN);
            }

            ConditionalFunctionHelper.AttachFunctions();
        }

        private static void AddPriorities(string sensor, params Target[] targets)
        {
            AddTargets(sensorPriorities, sensor, targets);
        }

        private static void AddConditionals(string sensor, params Target[] targets)
        {
            AddTargets(sensorConditionals, sensor, targets);
        }

        private static void AddTargets(Dictionary<string, List<Target>> map, string sensor, params Target[] targets)
        {
            if (!map.ContainsKey(sensor)) map.Add(sensor, new List<Target>());
            map[sensor].AddRange(targets);
        }

        private static List<string> FromAsResources(Dictionary<string, List<Target>> map, string sensor)
        {
            List<string> resources = new List<string>();
            foreach (Target target in map[sensor])
            {
                resources.Add(TargetHelper.AsResource(target));
            }
            return resources;
        }

        internal static List<string> PrioritiesFrom(string sensor)
        {
            return FromAsResources(sensorPriorities, sensor);
        }

        internal static List<string> ConditionalsFrom(ICollection<string> sensors)
        {
            List<string> conditionals = new List<string>();
            foreach (string sensor in sensors)
            {
                conditionals.AddRange(FromAsResources(sensorConditionals, sensor));
            }
            return conditionals;
        }

        internal static List<BotController> ApplyTargetPriority(string targetPriority, BotController myself, List<BotController> them)
        {
            return priorityFunctions[TargetHelper.Parse(targetPriority)](myself, them);
        }

        internal static bool ApplyTargetConditional(string targetConditional, BotController myself, BotController target)
        {
            return conditionalFunctions[TargetHelper.Parse(targetConditional)](myself, target);
        }
    }
}
