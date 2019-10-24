using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Interface Objects/Scale Controllers/Sliced Sprite")]
[RequireComponent(typeof(SpriteRenderer))]
public class SlicedSpriteScaleController : ScaleController
{
    private SpriteRenderer sprite;

    protected override void Start()
    {
        base.Start();
        sprite = GetComponent<SpriteRenderer>();
    }

    protected override void ApplyScale(Vector2 scale)
    {
        sprite.size = scale;
    }
}
