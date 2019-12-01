using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBlock : DropdownBlock
{
    public override bool IsValid()
    {
        return BehaviorLabController.CurrentMatchingEquipmentAsResources(PartType.Transport).Count > 0;
    }

    protected override List<string> Supplier()
    {
        return new List<string>{ "forward", "backward", "up", "down" }; // Forward/backward as opposed to left/right since teams could be on either side
    }

    protected override string Type() => "Move";
    protected override string DropdownAttributeKey() => "direction";
}
