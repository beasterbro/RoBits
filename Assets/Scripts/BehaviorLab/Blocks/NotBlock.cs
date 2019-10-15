using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotBlock : Block
{
    protected override BehaviorData InnerEvaluate()
    {
        return BehaviorData.EMPTY;
    }
}
