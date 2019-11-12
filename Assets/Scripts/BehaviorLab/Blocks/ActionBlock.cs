using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Interface Objects/Blocks/Action")]
public class ActionBlock : Block
{
    public override ReturnType OutputType()
    {
        return ReturnType.EMPTY;
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
        return "action";
    }

    protected override int[] ChunkSizes()
    {
        return new int[0];
    }
}
