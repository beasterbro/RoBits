using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotInputComponent : BlockComponent
{
    [SerializeField] private Block slot;
    [SerializeField] private ReturnType expectedOutputType;

    public ReturnType GetExpectedOutputType()
    {
        return this.expectedOutputType;
    }

    public bool IsFull()
    {
        return slot != null;
    }

    public bool MatchesOutputType(Block block)
    {
        return this.expectedOutputType == block.GetOutputType();
    }

    public void Push(Block block)
    {
        if (!IsFull() && MatchesOutputType(block))
        {
            slot = block;
            // TODO: probably some other stuff to setup the block
        }
    }

    public Block Pop()
    {
        Block result = slot;
        this.slot = null;
        return result;
    }

    public BehaviorData Evaluate()
    {
        return IsFull() ? slot.Evaluate() : BehaviorData.EMPTY;
    }

    public bool IsValid()
    {
        return IsFull() && MatchesOutputType(slot);
    }
}
