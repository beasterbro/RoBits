using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[AddComponentMenu("Interface Objects/Blocks/If")]
public class IfBlock : Block
{
    [SerializeField] private SlotInputComponent condition;
    [SerializeField] private ChunkInputComponent thenChunk;

    protected override void Start()
    {
        base.Start();
        if (condition.GetExpectedOutputType() != ReturnType.LOGICAL)
        {
            throw new ArgumentException("Condition MUST be a logical slot component!!!");
        }

        thenChunk.LinkScaleController(this.scaleController);
    }

    public override ReturnType OutputType()
    {
        return ReturnType.EMPTY;
    }

    protected override BehaviorData InnerEvaluate()
    {
        if (condition.Evaluate().GetLogical())
        {
            thenChunk.Evaluate();
        }
        return BehaviorData.EMPTY;
    }

    public override bool IsValid()
    {
        return condition != null && condition.GetExpectedOutputType() == ReturnType.LOGICAL
            && condition.IsValid();
    }

    protected override List<Block> Children()
    {
        List<Block> children = new List<Block>();
        children.Add(condition.Peek());
        children.AddRange(thenChunk.Elements());
        return children;
    }

    protected override string Type()
    {
        return "if";
    }

    protected override int[] ChunkSizes()
    {
        return new int[] { thenChunk.Elements().Count };
    }
}
