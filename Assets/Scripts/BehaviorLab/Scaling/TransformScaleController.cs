using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Interface Objects/Scale Controllers/Transform (default)")]
public class TransformScaleController : ScaleController
{
    protected override void ApplyScale(Vector2 scale)
    {
        this.transform.localScale = (Vector3)scale + Vector3.forward * this.transform.localScale.z;
    }
}
