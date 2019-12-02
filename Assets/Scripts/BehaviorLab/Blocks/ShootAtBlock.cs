using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShootAtBlock : DropdownBlock
{
    private static List<ShootAtBlock> subscribers = new List<ShootAtBlock>();

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

    public override bool IsValid()
    {
        return Supplier().Count > 0; // Supplier returns no values if there are no guns attached to the bot
    }

    protected override List<string> Supplier()
    {
        return new List<string>(BehaviorLabController.CurrentMatchingEquipmentAsResources(PartType.Weapon, true));
    }

    protected override string Type() => "ShootAt";
    protected override string DropdownAttributeKey() => "weapon";

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
            }
        }
        
        Redraw();
    }

}
