using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Interface Objects/Blocks/Logic/And")]
public class AndBlock : LogicBlock
{
    
    // TODO: Add override for MakeNewInfo
    
    protected override bool Aggregate(bool first, bool next)
    {
        return first && next;
    }

    protected override string Type() => "And";
    
}
