using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Interface Objects/Blocks/Compare")]
public class ComparissonBlock : Block
{
    [SerializeField] private CompareType type;
    private enum CompareType
    {
        LESS, LESS_EQUAL, EQUAL, MORE_EQUAL, MORE
    }

    public override ReturnType OutputType()
    {
        return ReturnType.LOGICAL;
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
        return type.ToString().ToLower();
    }

    protected override int[] ChunkSizes()
    {
        return new int[0];
    }
}
