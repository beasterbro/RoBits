﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Interface Objects/Interface Object")]
[RequireComponent(typeof(ScaleController))]
public class InterfaceObject : MonoBehaviour
{
    // Note: container should not be changed except when an object is independent of its container
    [SerializeField] private InterfaceObject container;
    protected ScaleController scaleController;

    protected virtual void Start()
    {
        scaleController = GetComponent<ScaleController>();
    }

    public InterfaceObject GetContainer()
    {
        return container;
    }

    public void SetContainer(InterfaceObject container)
    {
        this.container = container;
    }

    public bool IsTopLevel()
    {
        return this.container == null;
    }

    // TODO: this should be fixed to only give Blocks to drag and drop
    // Default implementation is to select the top level object
    protected virtual void OnGrab()
    {
        if (this.IsTopLevel())
        {
            DragAndDropController.Instance().Grab(this);
        }
        else
        {
            this.container.OnGrab();
        }
    }

    // Default implementation is to reset position when dropped on top of another interface object
    public virtual void OnDrop()
    {
        DragAndDropController.Instance().ResetDrop();
    }

    protected virtual void OnOver() { }

    protected virtual void OnExit() { }

    protected void OnMouseOver()
    {
        DragAndDropController.Instance()?.HoverOn(this);
        this.OnOver();
    }

    protected void OnMouseExit()
    {
        DragAndDropController.Instance().HoverOff(this);
        this.OnExit();
    }

    // Updates the object's physical display
    // Implementations of this SHOULD update the boundary object
    public virtual void Redraw()
    {
        if (!this.IsTopLevel())
        {
            this.container.Redraw();
        }
    }

    public Vector2 Scale()
    {
        return scaleController != null ? scaleController.Scale() : Vector2.zero;
    }

    public Boundary Bounds()
    {
        return scaleController != null ? scaleController.Bounds() : Boundary.NONE;
    }

    // Makes any necessary updates to the object's frame after a state change
    // Note any changes should recurse down the chain of objects
    public virtual void LayoutObject()
    {

    }

    // Allows a full layout update to be made in response to a state change anywhere
    // in the hierarchy. Finds the topmost InterfaceObject this object is connected
    // to in the drag-and-drop hierarchy, and calls layoutObject on it.
    public void LayoutHierarchy()
    {
        if (this.IsTopLevel())
        {
            this.LayoutObject();
        }
        else
        {
            this.container.LayoutHierarchy();
        }
    }
}
