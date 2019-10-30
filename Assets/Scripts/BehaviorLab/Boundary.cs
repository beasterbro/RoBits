using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Boundary
{
    public static readonly Boundary NONE = new Boundary(null, Vector2.zero);

    [SerializeField] private Transform transform;
    [SerializeField] private float width;
    [SerializeField] private float height;

    public Boundary(Transform transform) : 
        this (transform, transform.lossyScale.x, transform.lossyScale.y) { }

    public Boundary(Transform transform, Vector2 scale) : 
        this (transform, Mathf.Abs(scale.x), Mathf.Abs(scale.y)) { }

    public Boundary(Transform transform, float width, float height)
    {
        this.transform = transform;
        this.width = width;
        this.height = height;
    }

    public Vector3 TopLeft => transform.position; // for efficiency just returns point

    public Vector3 TopRight => ArbitraryPoint(1, 0);

    public Vector3 BottomLeft => ArbitraryPoint(0, 1);

    public Vector3 BottomRight => ArbitraryPoint(1, 1);

    // x and y are percentages and therefore are clamped between the range [0, 1]
    public Vector3 ArbitraryPoint(float x, float y)
    {
        return transform.position + new Vector3(width * PClamp(x), -height * PClamp(y), 0);
    }

    private static float PClamp(float percent)
    {
        return Mathf.Clamp(percent, 0, 1);
    }
}
