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

    protected string CurrentValue => dropdown.Current;

    protected override Dictionary<string, string> TypeAttributes()
    {
        Dictionary<string, string> attr = base.TypeAttributes();
        attr.Add(DropdownAttributeKey(), CurrentValue);
        return attr;
    }

    protected override void ApplyTypeAttributes()
    {
        base.ApplyTypeAttributes();
        dropdown.Current = info.TypeAttrs[DropdownAttributeKey()] ?? "";
    }

    protected abstract List<string> Supplier();
    protected abstract string DropdownAttributeKey();
}
