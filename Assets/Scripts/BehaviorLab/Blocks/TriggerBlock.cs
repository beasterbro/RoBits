using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Interface Objects/Blocks/Trigger")]
public class TriggerBlock : BodyBlock
{

    private TriggerInfo triggerInfo;

    [SerializeField] private TextMesh name;

    public BehaviorInfo BehaviorState()
    {
        var blockStates = States();
        blockStates.RemoveAll(state => state == null);
        return new BehaviorInfo(triggerInfo.ID, info.ID, blockStates.ToArray());
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
            name.text = triggerInfo.Name;
        }
    }

    public override void PositionConnections()
    {
        SetupScaleControllers();
        
        foreach (var blockId in info.InputIDs)
        {
            var block = BehaviorLabController.GetShared().GetBlockById(blockId);
            if (block != null)
            {
                bodyChunk.Add(block);
                block.PositionConnections();
            }
        }
        
        Redraw();
    }

}