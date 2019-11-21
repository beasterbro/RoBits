using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    protected override int[] ChunkSizes()
    {
        return new int[0];
    }
}
