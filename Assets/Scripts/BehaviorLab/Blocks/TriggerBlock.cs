using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Interface Objects/Blocks/Trigger")]
public class TriggerBlock : BodyBlock
{

    [SerializeField] private int id;
    private TriggerInfo triggerInfo;

    [SerializeField] private TextMesh name;

    public BehaviorInfo BehaviorState()
    {
        var initial = 0;
        var blockStates = States(initial);
        return new BehaviorInfo(triggerInfo.ID, initial, blockStates.ToArray());
    }

    protected override string Type() => "Trigger";

    protected override Dictionary<string, string> TypeAttributes()
    {
        var attrs = base.TypeAttributes();
        if (triggerInfo != null) attrs["triggerId"] = triggerInfo.ID.ToString();
        return attrs;
    }

    protected override void ApplyTypeAttributes()
    {
        base.ApplyTypeAttributes();
        if (info != null && info.TypeAttrs != null && info.TypeAttrs.ContainsKey("triggerId"))
        {
            triggerInfo = TriggerInfo.triggers[int.TryParse(info.TypeAttrs["triggerId"], out var ind) ? ind : 0];
            id = triggerInfo.ID;
            name.text = triggerInfo.Name;
        }
    }

}