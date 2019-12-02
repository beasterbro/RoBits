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
        return new List<string>{ "stop", "forward", "backward", "left", "right" }; // left/right indicate a rotation of the bot
    }

    protected override string Type() => "Move";
    protected override string DropdownAttributeKey() => "direction";
}
