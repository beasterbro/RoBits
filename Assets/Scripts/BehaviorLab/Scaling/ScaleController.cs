using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Interface Objects/Scale Controllers/Abstract")]
public abstract class ScaleController : MonoBehaviour
{
    [SerializeField] private Vector2 offset;
    [SerializeField] private Vector2 minimum;
    [SerializeField] private Boundary boundary;

    protected virtual void Start()
    {
        boundary = ScaledBoundary(Vector2.zero);
    }

    private Boundary ScaledBoundary(Vector2 scale)
    {
        return new Boundary(transform, OffsetScale(scale));
    }

    private Vector2 OffsetScale(Vector2 scale)
    {
        return Vector2.Max(scale, minimum) + offset;
    }

    public Boundary Bounds()
    {
        return boundary;
    }

    public Vector2 Scale()
    {
        return boundary.TopRight - boundary.BottomLeft;
    }

    public void UpdateScale(Vector2 scale)
    {
        scale.x = NonNegative(scale.x);
        scale.y = NonNegative(scale.y);
        boundary = ScaledBoundary(scale);
        this.ApplyScale(OffsetScale(scale));
    }

    private float NonNegative(float num)
    {
        return Mathf.Max(num, 0f);
    }

    protected abstract void ApplyScale(Vector2 scale);
}
