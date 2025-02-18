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
        attr.Add("sensor", sensor);
        return attr;
    }

    protected override void ApplyTypeAttributes()
    {
        base.ApplyTypeAttributes();
        sensor = info.TypeAttrs["sensor"];
        nthPlaceInput.text = info.TypeAttrs["n"] ?? "1";
    }

    public override bool IsValid()
    {
        return BehaviorLabController.CurrentMatchingEquipmentAsResources(PartType.Sensor).Contains(sensor);
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

    protected override string Type()
    {
        if (sensor == "ProximitySensor") return "TargetProx";
        if (sensor == "VisionSensor") return "TargetVision";
        return "Target";
    }

    protected override string DropdownAttributeKey() => "priority";
}
