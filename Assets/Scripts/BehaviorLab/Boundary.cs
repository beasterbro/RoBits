using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundary
{
    private Vector3 topLeft;
    private float width;
    private float height;

    public Boundary(Transform transform) : 
        this (transform.position, transform.lossyScale.x, transform.lossyScale.y) { }

    public Boundary(Vector3 topLeft, float width, float height)
    {
        this.topLeft = topLeft;
        this.width = width;
        this.height = height;
    }

    public Vector3 TopLeft()
    {
        return topLeft; // for efficiency just returns point
    }

    public Vector3 TopRight()
    {
        return ArbitraryPoint(1, 0);
    }

    public Vector3 BottomLeft()
    {
        return ArbitraryPoint(0, 1);
    }

    public Vector3 BottomRight()
    {
        return ArbitraryPoint(1, 1);
    }

    // x and y are percentages and therefore are clamped between the range [0, 1]
    public Vector3 ArbitraryPoint(float x, float y)
    {
        return topLeft + new Vector3(width * Clamp(x), -height * Clamp(y), 0);
    }

    private float Clamp(float percent)
    {
        return Mathf.Clamp(percent, 0, 1);
    }
}
