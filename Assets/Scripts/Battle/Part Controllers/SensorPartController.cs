using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SensorPartController : PartController
{
    public override bool IsActor()
    {
        return false;
    }

    public abstract void Observe();
}