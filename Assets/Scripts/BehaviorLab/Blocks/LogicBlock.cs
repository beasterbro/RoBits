using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class LogicBlock : Block
{
    [SerializeField] private List<SlotInputComponent> conditions;
    [SerializeField] private int idealConditionCount;

    protected override void Start()
    {
        base.Start();
        foreach (SlotInputComponent condition in conditions)
        {
            if (condition.GetExpectedOutputType() != ReturnType.LOGICAL)
            {
                throw new ArgumentException("Conditions MUST be logical slot components!!!");
            }
        }
    }

    public override ReturnType OutputType()
    {
        return ReturnType.LOGICAL;
    }

    protected override BehaviorData InnerEvaluate()
    {
        return conditions.Count > 0 ? new BehaviorData(AggregatedConditions()) : BehaviorData.EMPTY;
    }

    private bool AggregatedConditions()
    {
        // Assert that conditions has AT LEAST 1 element
        bool result = conditions[0];
        for (int i = 1; i < conditions.Count; i++)
        {
            result = Aggregate(result, conditions[i]);
        }
        return result;
    }

    protected abstract bool Aggregate(bool first, bool next);

    public override bool IsValid()
    {
        return conditions.Count == idealConditionCount && AllConditionsValid();
    }

    private bool AllConditionsValid()
    {
        foreach (SlotInputComponent condition in conditions)
        {
            if (condition.GetExpectedOutputType() != ReturnType.LOGICAL || !condition.IsValid())
            {
                return false;
            }
        }
        return true;
    }

    protected override List<Block> Children()
    {
        List<Block> children = new List<Block>();
        foreach (SlotInputComponent condition in conditions)
        {
            children.Add(condition.Peek());
        }
        return children;
    }

    protected override int[] ChunkSizes()
    {
        return new int[0];
    }
}
