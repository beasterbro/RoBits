using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Interface Objects/Blocks/Logic/Or")]
public class OrBlock : LogicBlock
{
    protected override bool Aggregate(bool first, bool next)
    {
        return first || next;
    }

    protected override string Type() => "Or";

}
