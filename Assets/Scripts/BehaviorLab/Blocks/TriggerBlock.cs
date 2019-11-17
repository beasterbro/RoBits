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

    protected override List<Block> Children()
    {
        return new List<Block>();
    }

    protected override string Type()
    {
        return "trigger";
    }

    protected override int[] ChunkSizes()
    {
        return new int[0];
    }
}
