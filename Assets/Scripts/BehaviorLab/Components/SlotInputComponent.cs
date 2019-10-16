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

    // TODO: this should be fixed to only give Blocks to drag and drop
    // Default implementation is to select the top level object
    protected override void OnGrab()
    {
        if (this.IsFull())
        {
            DragAndDropController.Instance().Grab(Pop(), this);
        }
    }

    // Default implementation is to reset position when dropped on top of another interface object
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
