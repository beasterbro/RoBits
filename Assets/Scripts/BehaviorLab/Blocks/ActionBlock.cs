using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Interface Objects/Blocks/Action")]
public abstract class ActionBlock : Block
{
    public override ReturnType OutputType()
    {
        return ReturnType.EMPTY;
    }

    protected override BehaviorData InnerEvaluate()
    {
        return BehaviorData.EMPTY;
    }

    protected override List<Block> Children()
    {
        return new List<Block>();
    }

    protected override int[] ChunkSizes()
    {
        return new int[0];
    }
}