using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IfBlock : Block
{
    protected override BehaviorData InnerEvaluate()
    {
        return BehaviorData.EMPTY;
    }
}
