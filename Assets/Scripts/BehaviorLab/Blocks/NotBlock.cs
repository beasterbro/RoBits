using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[AddComponentMenu("Interface Objects/Blocks/Logic/Not")]
public class NotBlock : LogicBlock
{
    protected override bool Aggregate(bool first, bool next)
    {
        return !first;
    }

    protected override string Type() => "Not";

    public override void PositionConnections()
    {
        base.PositionConnections();
    }

}
