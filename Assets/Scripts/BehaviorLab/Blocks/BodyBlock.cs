using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Interface Objects/Blocks/Body")]
public class BodyBlock : Block
{
    [SerializeField] private ChunkInputComponent bodyChunk;

    protected override void Start()
    {
        base.Start();
        bodyChunk.LinkScaleController(this.scaleController);
    }

    public override bool IsValid()
    {
        return bodyChunk.IsValid();
    }

    public override ReturnType OutputType()
    {
        return ReturnType.EMPTY;
    }

    protected override List<Block> Children()
    {
        return new List<Block>(bodyChunk.Elements());
    }

    protected override int[] ChunkSizes()
    {
        return new int[] { bodyChunk.Elements().Count };
    }

    protected override BehaviorData InnerEvaluate()
    {
        bodyChunk.Evaluate();
        return BehaviorData.EMPTY;
    }

    protected override string Type()
    {
        return "body";
    }
}
