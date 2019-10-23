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
        return this.expectedOutputType == block.OutputType();
    }

    public void Push(Block block)
    {
        if (!IsFull() && MatchesOutputType(block))
        {
            slot = block;
            block.SetContainer(this);
            block.transform.position = this.transform.position;
            block.transform.parent = this.transform;
            // TODO: probably some other stuff to setup the block
        }
    }

    public Block Pop()
    {
        Block result = slot;
        result.SetContainer(null);
        result.transform.parent = null;
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

    // Override meant to remove item from slot on grab
    protected override void OnGrab()
    {
        if (this.IsFull())
        {
            DragAndDropController.Instance().Grab(Pop(), this);
        }
    }

    // Override meant to add drop to this slot if there is space available
    public override void OnDrop()
    {
        if (this.IsFull())
        {
            DragAndDropController.Instance().ResetDrop();
        }
        else
        {
            Push(DragAndDropController.Instance().Drop());
        }
    }
}
