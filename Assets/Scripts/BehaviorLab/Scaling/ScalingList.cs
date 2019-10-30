using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScalingList<T> : IList<T> where T : InterfaceObject
{
    [SerializeField] internal List<T> elements;
    [SerializeField] private readonly bool isVertical;
    [SerializeField] private ScaleController scaleController;
    [SerializeField] private Transform transform;

    public ScalingList (Transform transform, bool isVertical)
    {
        elements = new List<T>();
        this.isVertical = isVertical;
        this.transform = transform;
    }

    public ScalingList (Transform transform) : this (transform, true) { }

    public void LinkScaleController(ScaleController scaleController)
    {
        this.scaleController = scaleController;
        UpdateScale();
    }

    private void UpdateScale()
    {
        if (scaleController != null)
        {
            Vector2 scale = Vector2.zero;
            elements.ForEach(element =>
            {
                element.transform.position = this.transform.position - (Vector3)ProjectScale(scale);
                scale += element.Scale();
            });
            scaleController.UpdateScale(ProjectScale(scale));
        }
    }

    private Vector2 ProjectScale(Vector2 scale)
    {
        return scale * (this.isVertical ? Vector2.up : Vector2.right);
    }

    public T this[int index]
    {
        get => elements[index];
        set
        {
            elements[index] = value;
            UpdateScale();
        }
    }

    public void Add(T item) { Insert(Count, item); }
    public void Insert(int index, T item)
    {
        elements.Insert(index, item);
        UpdateScale();
    }

    public void RemoveAt(int index)
    {
        elements.RemoveAt(index);
        UpdateScale();
    }

    public void Clear()
    {
        elements.Clear();
        UpdateScale();
    }

    public bool IsReadOnly => false;
    public int Count => elements.Count;
    public int IndexOf(T item) { return elements.IndexOf(item); }
    public bool Contains(T item) { return elements.Contains(item); }
    public void CopyTo(T[] array, int arrayIndex) { throw new NotImplementedException(); }
    public bool Remove(T item) { throw new NotImplementedException(); }
    public IEnumerator<T> GetEnumerator() { return elements.GetEnumerator(); }
    IEnumerator IEnumerable.GetEnumerator() { return elements.GetEnumerator(); }
    public int FindLastIndex(Predicate<T> p) { return elements.FindLastIndex(p); }
}
