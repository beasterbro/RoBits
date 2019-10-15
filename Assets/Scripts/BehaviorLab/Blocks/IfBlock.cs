using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class IfBlock : Block
{
    [SerializeField] private SlotInputComponent condition;
    [SerializeField] private ChunkInputComponent thenChunk;

    private void Start()
    {
        if (condition.GetExpectedOutputType() != ReturnType.LOGICAL)
        {
            throw new ArgumentException("Condition MUST be a logical slot component!!!");
        }
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
}
