using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotBlock : Block
{
    public override bool IsValid()
    {
        throw new System.NotImplementedException();
    }

    protected override BehaviorData InnerEvaluate()
    {
        return BehaviorData.EMPTY;
    }
}
