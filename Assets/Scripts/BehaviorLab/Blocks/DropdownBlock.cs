using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Interface Objects/Blocks/Dropdown")]
public abstract class DropdownBlock : ActionBlock
{
    // TODO: need to add current dropdown value to attributes of block during state creation!
    [SerializeField] private DropdownComponent dropdown;

    protected override void Start()
    {
        base.Start();
        UpdateItems();
    }

    public void UpdateItems()
    {
        dropdown.SetSupplier(Supplier);
    }

    protected string CurrentValue()
    {
        return dropdown.Current;
    }

    protected abstract List<string> Supplier();
}
