using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComparissonBlock : Block
{
    public override bool IsValid()
    {
        return false;
    }

    protected override BehaviorData InnerEvaluate()
    {
        return BehaviorData.EMPTY;
    }
}
