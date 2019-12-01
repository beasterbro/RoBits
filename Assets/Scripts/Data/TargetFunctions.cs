using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace TargetFunctions
{
    // Enum + helper class to provide ease of managing priority functions
    internal enum TargetPriority
    {
        UNKNOWN, RANGE_SHORT, RANGE_MEDIUM, RANGE_LONG, CLOSEST, FARTHEST, HEALTH_LOW, HEALTH_HIGH, SHIELDS_LOW, SHIELDS_HIGH, SHIELDS_DOWN
    }

    internal class TargetPriorityHelper
    {
        public static TargetPriority Parse(string priority)
        {
            foreach (TargetPriority targetPriority in Enum.GetValues(typeof(TargetPriority)))
            {
                if (ToString(targetPriority).Equals(priority.ToLower())) return targetPriority;
            }
            return TargetPriority.UNKNOWN;
        }

        internal static string ToString(TargetPriority targetPriority)
        {
            return targetPriority.ToString().ToLower().Replace("_", string.Empty);
        }
    }

    public delegate List<BotController> TargetFunction(BotController myself, List<BotController> them);

    // Class to provide the implementations of each target priority function
    internal class TargetFunctionHelper
    {
        internal static void AttachTargetFunctions()
        {
            AddFunction(TargetPriority.CLOSEST, Closest);
            AddFunction(TargetPriority.FARTHEST, Farthest);

            // TODO: AddFunction(TargetPriority.HEALTH_HIGH, null);
            // TODO: AddFunction(TargetPriority.HEALTH_LOW, null);

            // TODO: AddFunction(TargetPriority.RANGE_LONG, null);
            // TODO: AddFunction(TargetPriority.RANGE_MEDIUM, null);
            // TODO: AddFunction(TargetPriority.RANGE_SHORT, null);

            // TODO: AddFunction(TargetPriority.SHIELDS_DOWN, null);
            // TODO: AddFunction(TargetPriority.SHIELDS_HIGH, null);
            // TODO: AddFunction(TargetPriority.SHIELDS_LOW, null);
        }

        private static void AddFunction(TargetPriority target, TargetFunction func)
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
