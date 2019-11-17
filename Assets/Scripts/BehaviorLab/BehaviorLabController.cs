using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorLabController : MonoBehaviour
{
    [SerializeField] private TriggerBlock trigger;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            PrintBehavior();
        }
    }

    private void PrintBehavior()
    {
        Debug.Log(trigger.BehaviorState());
    }
}
