using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sensors;

[AddComponentMenu("Behavior Lab/Sensor/Sensor Bar")]
public class SensorBarController : MonoBehaviour
{
    public static SensorBarController Instance { get; private set; }
    
    [SerializeField] private LayoutGroup elementContainer;

    [SerializeField] private SensorGroupController sensorGroupPrefab;
    
    void Start()
    {
        Instance = this;
    }

    public void Load(List<SensorType> sensors)
    {
        SensorGroupController sensorGroup;
        foreach (SensorType sensor in sensors)
        {
            sensorGroup = Instantiate(sensorGroupPrefab, elementContainer.transform);
            sensorGroup.Load(sensor);
        }
    }

    public static List<SensorType> CurrentBotSensors()
    {
        List<SensorType> sensors = new List<SensorType>();
        // Loop through current bot's parts and pull out each sensor included
        foreach (PartInfo part in BehaviorLabController.currentBot.Equipment)
        {
            if (part.PartType == PartType.Sensor)
            {
                sensors.Add(SensorTypeHelper.Parse(part.Name));
            }
        }
        return sensors;
    }
}
