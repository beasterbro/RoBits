using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("Behavior Lab/Sensor/Trigger Bar Item")]
[RequireComponent(typeof(LayoutElement))]
public class UITriggerController : MonoBehaviour
{
    [SerializeField] private Text title;
    private int triggerId;

    public void UpdateValues(string name, int id)
    {
        this.name = name;
        this.title.text = this.name;
        this.triggerId = id;
    }

    public void OnSelect()
    {
        BehaviorLabController.UpdateTrigger(this.triggerId);
    }
}
