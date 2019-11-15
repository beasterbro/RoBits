using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Note: This is used as the top level block in a behavior scene
[AddComponentMenu("Interface Objects/Blocks/Trigger")]
public class TriggerBlock : BodyBlock
{
    [SerializeField] private int id;
    private TriggerInfo info;

    [SerializeField] private TextMesh name;

    protected override void Start()
    {
        base.Start();
        UpdateTrigger(this.id);
    }

    public void UpdateTrigger(int id)
    {
        this.id = id;
        this.info = TriggerManager.GetTrigger(id);
        this.name.text = this.info.Name;
    }

    public BehaviorInfo BehaviorState()
    {
        int initial = 0;
        List<BlockInfo> blockStates = States(initial);
        return new BehaviorInfo(info.ID, initial, blockStates.ToArray());
    }

    protected override string Type()
    {
        return "trigger";
    }
}
