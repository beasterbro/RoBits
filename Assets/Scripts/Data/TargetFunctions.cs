using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace TargetFunctions
{
    // Enum + helper class to provide ease of managing priority functions
    internal enum Target
    {
        UNKNOWN, IS_TOO_CLOSE, IN_SHORT_RANGE, IN_MEDIUM_RANGE, IN_LONG_RANGE, CLOSEST, FARTHEST, LOWEST_HEALTH, HIGHEST_HEALTH, SHIELDS_ARE_LOW, SHIELDS_ARE_HIGH, SHIELDS_ARE_DOWN
    }

    internal class TargetHelper
    {
        public static Target Parse(string targetString)
        {
            foreach (Target target in Enum.GetValues(typeof(Target)))
            {
                if (Ignorable(target.ToString()).Equals(Ignorable(targetString))) return target;
            }
            return Target.UNKNOWN;
        }

        internal static string Ignorable(string str)
        {
            return AsResource(str).Replace(" ", string.Empty);
        }

        private static string AsResource(string str)
        {
            return str.ToLower().Replace("_", " ");
        }

        internal static string AsResource(Target target)
        {
            return AsResource(target.ToString());
        }

        internal static float Distance(BotController from, BotController to)
        {
            return Vector3.Magnitude(from.transform.position - to.transform.position);
        }
    }

    public delegate List<BotController> PriorityFunction(BotController myself, List<BotController> them);
    public delegate bool ConditionalFunction(BotController myself, BotController target);

    // Class to provide the implementations of each target priority function
    internal class PriorityFunctionHelper
    {
        internal static void AttachFunctions()
        {
            AddFunction(Target.CLOSEST, Closest);
            AddFunction(Target.FARTHEST, Farthest);
            AddFunction(Target.LOWEST_HEALTH, LowHealth);
            AddFunction(Target.HIGHEST_HEALTH, HighHealth);
        }

        private static void AddFunction(Target target, PriorityFunction func)
        {
            TargetFunctionManager.priorityFunctions.Add(target, func);
        }

        private static List<BotController> Closest(BotController myself, List<BotController> them)
        {
            return ByDistance(myself, them, true);
        }

        private static List<BotController> Farthest(BotController myself, List<BotController> them)
        {
            return ByDistance(myself, them, false);
        }

        private static List<BotController> ByDistance(BotController myself, List<BotController> them, bool nearIsPriority)
        {
            List<BotController> working = new List<BotController>(them);
            working.Sort((x, y) =>
            {
                return (nearIsPriority ? 1 : -1) * TargetHelper.Distance(myself, x).CompareTo(TargetHelper.Distance(myself, y));
            });
            return working;
        }

        private static List<BotController> LowHealth(BotController myself, List<BotController> them)
        {
            return ByHealth(them, true);
        }

        private static List<BotController> HighHealth(BotController myself, List<BotController> them)
        {
            return ByHealth(them, false);
        }

        private static List<BotController> ByHealth(List<BotController> them, bool lowIsPriority)
        {
            List<BotController> working = new List<BotController>(them);
            working.Sort((x, y) =>
            {
                return (lowIsPriority ? 1 : -1) * x.currentHealth.CompareTo(y.currentHealth);
            });
            return working;
        }
    }

    // Class to provide the implementations of each target condition function
    internal class ConditionalFunctionHelper
    {
        internal static void AttachFunctions()
        {
            AddFunction(Target.IS_TOO_CLOSE, IsTooClose);
            AddFunction(Target.IN_SHORT_RANGE, InShortRange);
            AddFunction(Target.IN_MEDIUM_RANGE, InMediumRange);
            AddFunction(Target.IN_LONG_RANGE, InLongRange);
            AddFunction(Target.SHIELDS_ARE_DOWN, NoShields);
        }

        // Wraps a conditional function with a check that makes it return false if there is no target
        private static ConditionalFunction NotEmptyTarget(ConditionalFunction func)
        {
            return (myself, target) => target != null && func(myself, target);
        }

        private static void AddFunction(Target target, ConditionalFunction func)
        {
            TargetFunctionManager.conditionalFunctions.Add(target, NotEmptyTarget(func));
        }

        private static bool IsTooClose(BotController myself, BotController target)
        {
            ProximitySensor proxSensor = myself.GetComponentInChildren<ProximitySensor>();
            return proxSensor != null && DistanceBetween(myself, target, 0, proxSensor.minRange);
        }

        // TODO: How are the arbitrary numbers determined for the different ranges? Possibly values of BattleController??
        private static float shortToMediumRange = 1.5f;
        private static float mediumToLongRange = 4.5f;

        private static bool InShortRange(BotController myself, BotController target)
        {
            ProximitySensor proxSensor = myself.GetComponentInChildren<ProximitySensor>();
            float minRange = proxSensor != null ? proxSensor.minRange : 0;
            return DistanceBetween(myself, target, minRange, shortToMediumRange);
        }

        private static bool InMediumRange(BotController myself, BotController target)
        {
            return DistanceBetween(myself, target, shortToMediumRange, mediumToLongRange);
        }

        private static bool InLongRange(BotController myself, BotController target)
        {
            return DistanceBetween(myself, target, mediumToLongRange, float.PositiveInfinity);
        }

        private static bool DistanceBetween(BotController myself, BotController target, float min, float max)
        {
            float distance = TargetHelper.Distance(myself, target);
            return min <= distance && distance <= max;
        }

        private static bool NoShields(BotController myself, BotController target)
        {
            VisionSensor visSensor = myself.GetComponentInChildren<VisionSensor>();

            return visSensor == null || visSensor.BotHasPartOfType(target, "Reflective Armor") &&
                visSensor.PartsOnBotOfType<ReflectiveArmorController>(target, "Reflective Armor")[0].CanReflect();
        }
    }
}
