using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Interface Objects/Blocks/Target")]
public class TargetBlock : DropdownBlock
{
    [Tooltip("The sensor that provides this block with targets.")]
    [SerializeField] private string sensor;
    private int n = 1;

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
        return new BehaviorData(TargetingManager.NthTarget(n, CurrentValue(), myself));
    }

    protected override List<string> Supplier()
    {
        return TargetingManager.TargetingPriorities(sensor);
    }

    protected override string Type() => "Target";
}
