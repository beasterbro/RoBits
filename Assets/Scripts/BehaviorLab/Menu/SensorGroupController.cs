using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sensors;

[AddComponentMenu("Behavior Lab/Sensor/Sensor Bar Item")]
[RequireComponent(typeof(InterfaceObject))]
public class SensorGroupController : MonoBehaviour
{
    private ScalingList<InterfaceObject> uiTriggers;
    public InterfaceObject Interface { get; private set; }
    [SerializeField] private GameObject elementContainer;

    [SerializeField] private UITriggerController uiTriggerPrefab;
    
    void Start()
    {
        Interface = GetComponent<InterfaceObject>();
        uiTriggers = new ScalingList<InterfaceObject>(this.transform);
        uiTriggers.LinkScaleController(Interface.GetComponent<ScaleController>());
    }

    public void Load(SensorType sensor)
    {
        uiTriggers = new ScalingList<InterfaceObject>(elementContainer.transform);

        UITriggerController uiTrigger;
        foreach(TriggerInfo trigger in TriggerManager.GetTriggersFor(sensor))
        {
            uiTrigger = Instantiate(uiTriggerPrefab, elementContainer.transform);
            uiTriggers.Add(uiTrigger.Interface);
            uiTrigger.UpdateValues(trigger.Name, trigger.ID);
        }
    }
}
