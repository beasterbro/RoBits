using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Interface Objects/Scale Controllers/Constant")]
public class ConstantScaleController : ScaleController
{
    [SerializeField] private ScaleController onScale;

    protected override void ApplyScale(Vector2 scale)
    {
        // Don't update scale, just use current scale
        onScale.UpdateScale(this.Scale());
    }
}
