using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Interface Objects/Blocks/Trigger")]
public class TriggerBlock : Block
{
    [SerializeField] private ReturnType outputType = ReturnType.LOGICAL;

    public override ReturnType OutputType()
    {
        return outputType;
    }

    public override bool IsValid()
    {
        return false;
    }

    protected override BehaviorData InnerEvaluate()
    {
        return BehaviorData.EMPTY;
    }
}
