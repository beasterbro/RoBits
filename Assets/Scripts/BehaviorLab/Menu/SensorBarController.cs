using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sensors;

[AddComponentMenu("Behavior Lab/Sensor/Sensor Bar")]
public class SensorBarController : MonoBehaviour
{
    public static SensorBarController Instance { get; private set; }

    private ScalingList<InterfaceObject> sensorGroups;
    [SerializeField] private ScaleController scaleController;
    [SerializeField] private GameObject elementContainer;

    [SerializeField] private SensorGroupController sensorGroupPrefab;
    
    void Start()
    {
        Instance = this;
        sensorGroups = new ScalingList<InterfaceObject>(this.transform);
        sensorGroups.LinkScaleController(scaleController);
    }

    public void Load(List<SensorType> sensors)
    {
        sensorGroups = new ScalingList<InterfaceObject>(elementContainer.transform);

        SensorGroupController sensorGroup;
        foreach (SensorType sensor in sensors)
        {
            sensorGroup = Instantiate(sensorGroupPrefab, elementContainer.transform);
            sensorGroups.Add(sensorGroup.Interface);
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
