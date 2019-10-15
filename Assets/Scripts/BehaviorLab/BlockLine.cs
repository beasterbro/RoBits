using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockLine : InterfaceObject
{
    [SerializeField] private List<BlockComponent> components;

    // Calls updateStyle on its components
    public override void UpdateStyle()
    {
        base.UpdateStyle();
        components.ForEach(component => component.UpdateStyle());
    }

    // Calls layoutObject on each of its components, then positions them appropriately
    // and updates the line's frame
    public override void LayoutObject()
    {
        base.LayoutObject();
        components.ForEach(component => component.LayoutObject());

        // TODO: position appropriately
    }

    // Adds a new component to the line, and sets itself as the component's container
    public void AddComponent(BlockComponent component)
    {
        components.Add(component);
        // TODO: Not sure if the container part makes sense
    }

    // Removes a component from the line, and removes itself as the component's container
    public void RemoveComponent(BlockComponent component)
    {
        components.Remove(component);
        // TODO: Not sure if the container part makes sense
    }
}
