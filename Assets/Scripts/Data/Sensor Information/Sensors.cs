using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sensors
{
    public enum SensorType
    {
        PROXIMITY, LINE_OF_SIGHT, TARGETED, NULL
    }

    public class SensorTypeHelper
    {
        private readonly static List<SensorType> types;
        private readonly static List<SensorType> nonNullTypes;
        private readonly static Dictionary<string, SensorType> nameToType;

        static SensorTypeHelper()
        {
            types = new List<SensorType>(Enum.GetValues(typeof(SensorType)) as SensorType[]);

            nonNullTypes = new List<SensorType>(types);
            nonNullTypes.Remove(SensorType.NULL);

            nameToType = new Dictionary<string, SensorType>(types.Count);
            foreach (SensorType type in types)
            {
                nameToType.Add(type.ToString().ToLower(), type);
            }
        }

        public static SensorType Parse(string type)
        {
            if (type != null && nameToType.ContainsKey(type.ToLower()))
            {
                return nameToType[type.ToLower()];
            }
            else
            {
                return SensorType.NULL;
            }
        }

        public static List<SensorType> Types => new List<SensorType>(types);
        public static List<SensorType> NonNullTypes => new List<SensorType>(nonNullTypes);
    }
}
