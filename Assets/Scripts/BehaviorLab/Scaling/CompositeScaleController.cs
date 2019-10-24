using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Interface Objects/Scale Controllers/Composite")]
public class CompositeScaleController : ScaleController
{
    [SerializeField] private List<ScaleController> scaleControllers;

    protected override void ApplyScale(Vector2 scale)
    {
        foreach (ScaleController scaleController in scaleControllers)
        {
            scaleController.UpdateScale(scale);
        }
    }
}
