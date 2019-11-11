using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[AddComponentMenu("Interface Objects/Blocks/Not")]
public class NotBlock : LogicBlock
{
    protected override bool Aggregate(bool first, bool next)
    {
        return !first;
    }
}
