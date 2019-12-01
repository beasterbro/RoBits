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
        HashSet<string> weapons = new HashSet<string>();
        if (BehaviorLabController.GetShared().currentBot != null)
        {
            foreach (PartInfo part in BehaviorLabController.GetShared().currentBot.Equipment)
            {
                if (PartType.Weapon == part.PartType) weapons.Add(part.ResourceName);
            }
        }
        return new List<string>(weapons);
    }

    protected override string Type() => "ShootAt";
}
