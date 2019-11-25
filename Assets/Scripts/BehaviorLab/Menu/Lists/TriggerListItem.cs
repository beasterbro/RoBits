using UnityEngine.UI;

public class TriggerListItem : ListItem
{

    public override void LoadData(object data)
    {
        base.LoadData(data);
        if (data is TriggerInfo trigger) title.text = trigger.Name;
    }

}