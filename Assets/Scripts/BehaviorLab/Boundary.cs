using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Boundary
{
    [SerializeField] private Vector3 _topLeft;
    [SerializeField] private float width;
    [SerializeField] private float height;

    public Boundary(Transform transform) : 
        this (transform.position, transform.lossyScale.x, transform.lossyScale.y) { }

    public Boundary(Vector3 topLeft, Vector2 scale) : 
        this (topLeft, Mathf.Abs(scale.x), Mathf.Abs(scale.y)) { }

    public Boundary(Vector3 topLeft, float width, float height)
    {
        this._topLeft = topLeft;
        this.width = width;
        this.height = height;
    }

    public Vector3 TopLeft => _topLeft; // for efficiency just returns point

    public Vector3 TopRight => ArbitraryPoint(1, 0);

    public Vector3 BottomLeft => ArbitraryPoint(0, 1);

    public Vector3 BottomRight => ArbitraryPoint(1, 1);

    // x and y are percentages and therefore are clamped between the range [0, 1]
    public Vector3 ArbitraryPoint(float x, float y)
    {
        return _topLeft + new Vector3(width * PClamp(x), -height * PClamp(y), 0);
    }

    private static float PClamp(float percent)
    {
        return Mathf.Clamp(percent, 0, 1);
    }
}
