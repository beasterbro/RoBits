using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("Behavior Lab/Sensor/Trigger Bar Item")]
[RequireComponent(typeof(InterfaceObject))]
public class UITriggerController : MonoBehaviour
{
    [SerializeField] private Text text;
    public InterfaceObject Interface { get; private set; }
    private int triggerId;

    private void Start()
    {
        Interface = GetComponent<InterfaceObject>();
    }

    public void UpdateValues(string name, int id)
    {
        text.text = name;
        this.name = name;
        this.triggerId = id;
    }

    public void OnSelect()
    {
        BehaviorLabController.UpdateTrigger(this.triggerId);
    }
}
