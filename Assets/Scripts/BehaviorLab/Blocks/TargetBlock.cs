using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("Interface Objects/Blocks/Target")]
public class TargetBlock : DropdownBlock
{
    [Tooltip("The sensor that provides this block with targets.")]
    [SerializeField] private string sensor;
    [Tooltip("The input field that specifies the n for the nth target.")]
    [SerializeField] private InputField nthPlaceInput;

    protected override Dictionary<string, string> TypeAttributes()
    {
        Dictionary<string, string> attr = base.TypeAttributes();
        attr.Add("n", nthPlaceInput.text);
        return attr;
    }

    protected override void ApplyTypeAttributes()
    {
        base.ApplyTypeAttributes();
        nthPlaceInput.text = info.TypeAttrs["n"] ?? "1";
    }

    public override bool IsValid()
    {
        return Supplier().Count > 0;
    }

    public override ReturnType OutputType()
    {
        return ReturnType.BOT;
    }

    // TODO: this function will be separated from Block to be used independently in Battle
    public BehaviorData GetTargetedBot(BotController myself)
    {
        return new BehaviorData(TargetingManager.NthTarget(int.Parse(info.TypeAttrs["n"]), CurrentValue, myself));
    }

    protected override List<string> Supplier()
    {
        return TargetingManager.TargetingPriorities(sensor);
    }

    protected override string Type() => "Target";
    protected override string DropdownAttributeKey() => "priority";
}
