using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace TargetFunctions
{
    // Enum + helper class to provide ease of managing priority functions
    internal enum Target
    {
        UNKNOWN, RANGE_SHORT, RANGE_MEDIUM, RANGE_LONG, CLOSEST, FARTHEST, HEALTH_LOW, HEALTH_HIGH, SHIELDS_LOW, SHIELDS_HIGH, SHIELDS_DOWN
    }

    internal class TargetPriorityHelper
    {
        public static Target Parse(string priority)
        {
            foreach (Target targetPriority in Enum.GetValues(typeof(Target)))
            {
                if (ToString(targetPriority).Equals(priority.ToLower())) return targetPriority;
            }
            return Target.UNKNOWN;
        }

        internal static string ToString(Target targetPriority)
        {
            return targetPriority.ToString().ToLower().Replace("_", string.Empty);
        }
    }

    public delegate List<BotController> PriorityFunction(BotController myself, List<BotController> them);

    // Class to provide the implementations of each target priority function
    internal class PriorityFunctionHelper
    {
        internal static void AttachPriorityFunctions()
        {
            AddFunction(Target.CLOSEST, Closest);
            AddFunction(Target.FARTHEST, Farthest);
        }

        private static void AddFunction(Target target, PriorityFunction func)
        {
            PriorityManager.targetFunctions.Add(target, func);
        }

        private static float Distance(BotController from, BotController to)
        {
            return Vector3.Magnitude(from.transform.position - to.transform.position);
        }

        private static List<BotController> Closest(BotController myself, List<BotController> them)
        {
            return ByDistance(myself, them, true);
        }

        private static List<BotController> Farthest(BotController myself, List<BotController> them)
        {
            return ByDistance(myself, them, false);
        }

        private static List<BotController> ByDistance(BotController myself, List<BotController> them, bool isNear)
        {
            List<BotController> working = new List<BotController>(them);
            working.Sort((x, y) =>
            {
                return (isNear ? 1 : -1) * Distance(myself, x).CompareTo(Distance(myself, y));
            });
            return working;
        }
    }
}
