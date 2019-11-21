using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sensors;

[AddComponentMenu("Behavior Lab/Sensor/Sensor Bar Item")]
public class SensorGroupController : MonoBehaviour
{
    [SerializeField] private Text title;
    [SerializeField] private LayoutGroup elementContainer;

    [SerializeField] private UITriggerController uiTriggerPrefab;

    public void Load(SensorType sensor)
    {
        this.name = sensor.ToString();
        this.title.text = this.name;

        UITriggerController uiTrigger;
        foreach(TriggerInfo trigger in TriggerManager.GetTriggersFor(sensor))
        {
            uiTrigger = Instantiate(uiTriggerPrefab, elementContainer.transform);
            uiTrigger.UpdateValues(trigger.Name, trigger.ID);
        }
    }
}
