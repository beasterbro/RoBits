using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sensors
{
    public enum SensorType
    {
        PROXIMITY, LINE_OF_SIGHT, TARGETED, UNKNOWN
    }

    public class SensorTypeHelper
    {
        private readonly static List<SensorType> types;
        private readonly static List<SensorType> nonBlankTypes;
        private readonly static Dictionary<string, SensorType> nameToType;

        static SensorTypeHelper()
        {
            types = new List<SensorType>(Enum.GetValues(typeof(SensorType)) as SensorType[]);

            nonBlankTypes = new List<SensorType>(types);
            nonBlankTypes.Remove(SensorType.UNKNOWN);

            nameToType = new Dictionary<string, SensorType>(2 * types.Count);
            foreach (SensorType type in types)
            {
                nameToType.Add(PreProcessType(type.ToString()), type); // Default naming based on enum
            }

            // For any string naming pairs from database that DO NOT match enum directly
            {
                nameToType.Add("target", SensorType.TARGETED);
            }
        }

        // Returns a string that has been modified to allowing comparison while ignoring the following aspects
        //  underscores ("_") are treated like spaces (" ")
        //  uppercase letters ("A-Z") are treated like lowercase letters ("a-z")
        //  null types are treated like empty strings
        private static string PreProcessType(string initial)
        {
            return initial != null ? initial.ToLower().Replace('_', ' ') : "";
        }

        public static SensorType Parse(string type)
        {
            string processed = PreProcessType(type);
            if (nameToType.ContainsKey(processed))
            {
                return nameToType[processed];
            }
            else
            {
                return SensorType.UNKNOWN;
            }
        }

        public static List<SensorType> Types => new List<SensorType>(types);
        public static List<SensorType> KnownTypes => new List<SensorType>(nonBlankTypes);
    }
}
