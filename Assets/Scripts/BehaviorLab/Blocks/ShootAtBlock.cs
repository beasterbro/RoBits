using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAtBlock : DropdownBlock
{
    private static List<ShootAtBlock> subscribers = new List<ShootAtBlock>();

    public static void UpdateDropdownItems()
    {
        subscribers.ForEach(s => s.UpdateItems());
    }

    protected override void Start()
    {
        base.Start();
        subscribers.Add(this);
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
}
