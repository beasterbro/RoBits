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

    public override bool IsValid()
    {
        throw new System.NotImplementedException();
    }

    protected override List<string> Supplier()
    {
        throw new System.NotImplementedException(); // TODO: TargetingManager.ConditionalFunctions();
    }

    protected override string Type() => "Condition";
    protected override string DropdownAttributeKey() => "check";
}
