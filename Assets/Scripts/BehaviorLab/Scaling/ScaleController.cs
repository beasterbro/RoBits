using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Interface Objects/Scale Controllers/Abstract")]
public abstract class ScaleController : MonoBehaviour
{
    [SerializeField] private Vector2 offset;
    [SerializeField] private Boundary boundary;

    protected virtual void Start()
    {
        boundary = new Boundary(transform, offset);
    }

    private Vector2 OffsetScale(Vector2 scale)
    {
        return scale + offset;
    }

    public Vector2 Scale()
    {
        return boundary.TopRight - boundary.BottomLeft;
    }

    public void UpdateScale(Vector2 scale)
    {
        scale.x = NonNegative(scale.x);
        scale.y = NonNegative(scale.y);
        this.ApplyScale(OffsetScale(scale));
    }

    private float NonNegative(float num)
    {
        return Mathf.Max(num, 0f);
    }

    protected abstract void ApplyScale(Vector2 scale);
}
