﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceObject : MonoBehaviour
{
    // Note: container should not be changed except when a object is independent of its container
    [SerializeField] private InterfaceObject container;
    //[SerializeField] private Color color; // Handled by material

    public bool IsTopLevel()
    {
        return this.container == null;
    }

    // TODO: this should be fixed to only give Blocks to drag and drop
    // Default implementation is to select the top level object
    protected virtual void OnGrab(InterfaceObject child)
    {
        if (this.IsTopLevel())
        {
            DragAndDropController.Instance().Grab(this);
        }
        else
        {
            this.container.OnGrab(this);
        }
    }

    // Default implementation is to reset position when dropped on top of another interface object
    public virtual void OnDrop(Block obj)
    {
        DragAndDropController.Instance().ResetDrop();
    }

    private void OnMouseOver()
    {
        DragAndDropController.Instance().HoverOn(this);
    }

    private void OnMouseExit()
    {
        DragAndDropController.Instance().HoverOff(this);
    }

    // Updates the style of the object, without redrawing the whole thing
    public virtual void UpdateStyle()
    {
        // Still don't know what this means
    }

    // Updates the object's physical display
    public void Redraw()
    {

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