using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Note: This is used as the top level block in a behavior scene
[AddComponentMenu("Interface Objects/Blocks/Trigger")]
public class TriggerBlock : BodyBlock
{
    [SerializeField] private int id;

    public BehaviorInfo BehaviorState()
    {
        int initial = 0;
        List<BlockInfo> blockStates = States(initial);
        return new BehaviorInfo(id, initial, blockStates.ToArray());
    }

    protected override string Type()
    {
        return "trigger";
    }
}
