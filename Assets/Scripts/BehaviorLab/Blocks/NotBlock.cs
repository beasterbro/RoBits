using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[AddComponentMenu("Interface Objects/Blocks/Not")]
public class NotBlock : Block
{
    [SerializeField] private SlotInputComponent condition;

    protected override void Start()
    {
        base.Start();
        if (condition.GetExpectedOutputType() != ReturnType.LOGICAL)
        {
            throw new ArgumentException("Condition MUST be a logical slot component!!!");
        }
    }

    public override ReturnType OutputType()
    {
        return ReturnType.LOGICAL;
    }

    protected override BehaviorData InnerEvaluate()
    {
        return new BehaviorData(!condition.Evaluate().GetLogical());
    }

    public override bool IsValid()
    {
        return condition != null && condition.GetExpectedOutputType() == ReturnType.LOGICAL
            && condition.IsValid();
    }
}
