using UnityEngine.UI;
using UnityEngine;
using System;

public class TriggerListItem : ListItem
{
    public Action<TriggerListItem> onSelect;
    public Action<TriggerListItem> onShift;
    public Action<TriggerListItem> onDelete;

    public override void LoadData(object data)
    {
        base.LoadData(data);
        if (data is TriggerInfo trigger) title.text = trigger.Name;
    }

    public void OnSelect() => onSelect?.Invoke(this);

    public void OnShift() => onShift?.Invoke(this);

    public void OnDelete() => onDelete?.Invoke(this);
}