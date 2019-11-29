using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBlock : DropdownBlock
{
    public override bool IsValid()
    {
        if (BehaviorLabController.GetShared().currentBot != null)
        {
            foreach (PartInfo part in BehaviorLabController.GetShared().currentBot.Equipment)
            {
                if (PartType.Transport == part.PartType) return true;
            }
        }
        return false;
    }

    protected override List<string> Supplier()
    {
        return new List<string>{ "forward", "backward", "up", "down" }; // Forward/backward as opposed to left/right since teams could be on either side
    }

    protected override string Type() => "Move";
}
