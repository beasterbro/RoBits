﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Interface Objects/Blocks/Compare")]
public class ComparissonBlock : Block
{
    public override ReturnType OutputType()
    {
        return ReturnType.LOGICAL;
    }

    public override bool IsValid()
    {
        return false;
    }

    protected override BehaviorData InnerEvaluate()
    {
        return BehaviorData.EMPTY;
    }
}
