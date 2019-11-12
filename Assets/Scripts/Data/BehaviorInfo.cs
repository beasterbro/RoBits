using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorInfo
{

    private readonly int triggerId;
    private int entryBlockId; // TODO: Either have "Entry Block" be a thing that is just a chunk (a.k.a. {...}) OR switch this to be int[] since the trigger starts with a chunk component.
    private BlockInfo[] blocks;

    public BehaviorInfo(int triggerId, int entryBlockId, BlockInfo[] blocks)
    {
        this.triggerId = triggerId;
        this.entryBlockId = entryBlockId;
        this.blocks = blocks;
    }

    public int TriggerId => triggerId;
    public int EntryBlockId
    {
        get => entryBlockId;
        set => entryBlockId = value;
    }
    public BlockInfo[] Blocks
    {
        get => blocks;
        set => blocks = value;
    }
}
