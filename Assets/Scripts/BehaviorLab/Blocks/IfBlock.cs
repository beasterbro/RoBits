using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[AddComponentMenu("Interface Objects/Blocks/If")]
public class IfBlock : BodyBlock
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

    protected override BehaviorData InnerEvaluate()
    {
        if (condition.Evaluate().GetLogical())
        {
            base.InnerEvaluate();
        }
        return BehaviorData.EMPTY;
    }

    public override bool IsValid()
    {
        return condition != null && condition.GetExpectedOutputType() == ReturnType.LOGICAL
            && condition.IsValid() && base.IsValid();
    }

    protected override List<Block> Children()
    {
        List<Block> children = new List<Block>();
        children.Add(condition.Peek());
        children.AddRange(base.Children());
        return children;
    }

    protected override string Type() => "If";

    protected override int[] InputIDs()
    {
        var ids = new List<int>(base.InputIDs());
        ids.Insert(0, condition.IsFull() ? condition.Peek().info.ID : -1);
        return ids.ToArray();
    }

}
