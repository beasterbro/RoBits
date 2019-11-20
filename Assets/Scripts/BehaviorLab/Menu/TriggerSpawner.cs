using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSpawner : MonoBehaviour
{
    [SerializeField] private TriggerBlock prefab;

    public static TriggerBlock Trigger { get; private set; }
    
    void Start()
    {
        Vector3 position = transform.position;
        Trigger = Instantiate<TriggerBlock>(prefab);
        Trigger.transform.position = position;
        this.gameObject.SetActive(false);
    }
}
