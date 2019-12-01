using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Interface Objects/Blocks/Target")]
public class TargetBlock : DropdownBlock
{
    [Tooltip("The sensor that provides this block with targets.")]
    [SerializeField] private string sensor;
    private int n = 1;

    protected override Dictionary<string, string> TypeAttributes()
    {
        Dictionary<string, string> attr = base.TypeAttributes();
        attr.Add("n", n.ToString());
        return attr;
    }

    protected override void ApplyTypeAttributes()
    {
        base.ApplyTypeAttributes();
        n = int.Parse(info.TypeAttrs["n"] ?? "1");
    }

    public override bool IsValid()
    {
        return Supplier().Count > 0;
    }

    public override ReturnType OutputType()
    {
        return ReturnType.BOT;
    }

    public BehaviorData GetTargetedBot(BotController myself)
    {
        return new BehaviorData(TargetingManager.NthTarget(n, CurrentValue, myself));
    }

    protected override List<string> Supplier()
    {
        return TargetingManager.TargetingPriorities(sensor);
    }

    protected override string Type() => "Target";
    protected override string DropdownAttributeKey() => "priority";
}
