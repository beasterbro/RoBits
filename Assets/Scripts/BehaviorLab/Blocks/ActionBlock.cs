﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBlock : Block
{
    public override ReturnType OutputType()
    {
        return ReturnType.EMPTY;
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
