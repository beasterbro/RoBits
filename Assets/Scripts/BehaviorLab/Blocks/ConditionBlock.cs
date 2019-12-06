using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[AddComponentMenu("Interface Objects/Blocks/Condition")]
public class ConditionBlock : DropdownBlock
{
    private static List<ConditionBlock> subscribers = new List<ConditionBlock>();

    [SerializeField] private SlotInputComponent target;

    public static void UpdateDropdownItems()
    {
        subscribers.ForEach(s => s.UpdateItems());
    }

    protected override void Start()
    {
        base.Start();
        subscribers.Add(this);
        if (target.GetExpectedOutputType() != ReturnType.BOT)
        {
            throw new ArgumentException("Target MUST be a bot slot component!!!");
        }
    }

    public override ReturnType OutputType()
    {
        return ReturnType.LOGICAL;
    }

    protected override int[] InputIDs()
    {
        var ids = new List<int>();
        ids.Add(target.IsFull() ? target.Peek().info.ID : -1);
        return ids.ToArray();
    }

    protected override List<Block> Children()
    {
        List<Block> children = new List<Block>();
        children.Add(target.Peek());
        return children;
    }

    public override void PositionConnections()
    {
        SetupScaleControllers();

        if (info.InputIDs.Length > 0)
        {
            var input = BehaviorLabController.GetShared().GetBlockById(info.InputIDs[0]);
            if (input != null)
            {
                target.Push(input);
                input.PositionConnections();
            }
        }

        Redraw();
    }

    public override bool IsValid()
    {
        return target != null && target.GetExpectedOutputType() == ReturnType.BOT
            && target.IsValid();
    }

    protected override List<string> Supplier()
    {
        return TargetingManager.TargetingConditionals();
    }

    protected override string Type() => "Condition";
    protected override string DropdownAttributeKey() => "check";
}
