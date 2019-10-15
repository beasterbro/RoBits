using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBlock : Block
{
    protected override BehaviorData InnerEvaluate()
    {
        return BehaviorData.EMPTY;
    }
}
