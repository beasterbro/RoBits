using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceObject : MonoBehaviour
{
    [SerializeField] private InterfaceObject container;
    //[SerializeField] private Color color; // Handled by material

    public bool IsTopLevel()
    {
        return this.container == null;
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
